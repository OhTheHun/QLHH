import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import {
  DxButtonModule,
  DxDataGridModule,
  DxDateBoxModule,
  DxSelectBoxModule
} from "devextreme-angular";
import notify from "devextreme/ui/notify";
import { confirm } from "devextreme/ui/dialog";
import { exportDataGrid } from 'devextreme/excel_exporter';
import { saveAs } from 'file-saver';
import * as ExcelJS from 'exceljs';
import { Router } from "@angular/router";
import { HttpClient, HttpClientModule } from "@angular/common/http";
import { AuthService } from "../../shared/services";

/* ===== INTERFACE ===== */

export interface NhaCungCap {
  nhaCungCapId: string;
  tenNhaCungCap: string;
  diaChi?: string;
  email?: string;
  sdt?: string;
}
export interface Kho {
  khoId: string;
  tenKho: string;
  diaChi: string;
}
export interface SanPham {
  sanPhamId: string;
  danhMucId: string;
  tenSanPham: string;
  hinhAnh?: string;
  donGia: number;
  trangThai: boolean;
}

interface ChiTietPhieuNhap {
  SanPhamId: string;
  SoLuong: number;
  DonGia: number;
  NgayNhap: Date;
}

interface SanPhamByKhoId {
  sanPhamId: string;
  tenSanPham: string;
  soLuong: number;
}
@Component({
  selector: 'app-phieu-nhap',
  standalone: true,
  imports: [
    CommonModule,
    HttpClientModule,
    DxDataGridModule,
    DxButtonModule,
    DxSelectBoxModule,
    DxDateBoxModule
  ],
  templateUrl: './PhieuNhap.component.html',
  styleUrls: ['./PhieuNhap.component.scss']
})
export class PhieuNhapComponent implements OnInit {

  /* ===== API ===== */
  private apiNhaCungCap = 'https://localhost:7161/api/person/Nhacungcap';
  private apiSanPham = 'https://localhost:7161/api/products/SanPham';
  private apiCreatePhieuNhap = 'https://localhost:7161/api/product/phieu-nhap';
  private apiKho = 'https://localhost:7161/api/products/Kho';
  private apiSanPhamByKhoId = 'https://localhost:7161/api/products/product-byKhoId';
  constructor(
    private router: Router,
    private authService: AuthService,
    private http: HttpClient
  ) {}
  
  getSoLuongTon(sanPhamId: string): number {
  const sp = this.sanPhamByKhoDs.find(x => x.sanPhamId === sanPhamId);
  return sp ? sp.soLuong : 0;
}

  today = new Date();

  nhaCungCapId = '';
  khoId = '';
  sanPhamId = '';
  nhaCungCapDs: NhaCungCap[] = [];
  khoDs: Kho[] = [];
  sanPhamDs: SanPham[] = [];
  sanPhamByKhoDs: SanPhamByKhoId[] = [];
  chiTietPhieuNhap: ChiTietPhieuNhap[] = [];
  tongTien = 0;

  ngOnInit(): void {
    const user = this.authService.user;
    if (!user?.userId) {
      notify('Chưa đăng nhập', 'error', 2000);
      return;
    }

    this.loadNhaCungCap();
    this.loadKho();
    this.loadSanPhamByKhoId(this.khoId);
  }

  /* ===== LOAD NCC ===== */
  loadNhaCungCap() {
    this.http.get<NhaCungCap[]>(this.apiNhaCungCap).subscribe({
      next: res => this.nhaCungCapDs = res,
      error: () => notify('Không load được nhà cung cấp', 'error', 2000)
    });
  }

  /* ===== LOAD SẢN PHẨM ===== */

  loadKho() {
    this.http.get<Kho[]>(this.apiKho).subscribe({
      next: res => this.khoDs = res,
      error: () => notify('Không load được sản phẩm', 'error', 2000)
    });
  }
  loadSanPhamByKhoId(khoId: string) {
  if (!khoId) {
    this.sanPhamByKhoDs = [];
    return;
  }
  this.http
    .get<SanPhamByKhoId[]>(
      `${this.apiSanPhamByKhoId}?khoId=${khoId}`
    )
    .subscribe({
      next: res => {
        this.sanPhamByKhoDs = res;
      },
      error: () => notify('Không load được sản phẩm theo kho', 'error', 2000)
    });
}


  /* ===== TÍNH TIỀN ===== */
  calcThanhTien = (row: ChiTietPhieuNhap) =>
    (row.SoLuong || 0) * (row.DonGia || 0);

  recalcTotal() {
    this.tongTien = this.chiTietPhieuNhap.reduce(
      (s, x) => s + this.calcThanhTien(x), 0
    );
  }

  /* ===== SAVE ===== */
  async savePhieuNhap() {
  const user = this.authService.user;
  if (!user?.userId) {
    notify('Chưa đăng nhập', 'error', 2000);
    return;
  }

  if (!this.nhaCungCapId) {
    notify('Chưa chọn nhà cung cấp', 'error', 2000);
    return;
  }

  if (!this.chiTietPhieuNhap.length) {
    notify('Phiếu nhập chưa có sản phẩm', 'error', 2000);
    return;
  }

  const ok = await confirm('Xác nhận nhập kho?', 'Xác nhận');
  if (!ok) return;

  // Tạo payload phiếu nhập
  const payload = {
    userId: user.userId,
    nhaCungCapId: this.nhaCungCapId,
    chiTietPhieuNhap: this.chiTietPhieuNhap.map(x => ({
      sanPhamId: x.SanPhamId,
      soLuong: x.SoLuong,
      donGia: x.DonGia,
      ngayNhap: x.NgayNhap
    }))
  };

  try {
    await this.http.post(this.apiCreatePhieuNhap, payload).toPromise();
    notify('Lưu phiếu nhập thành công', 'success', 2000);
    const updateKhoPayload = this.chiTietPhieuNhap.map(x => ({
      sanPhamId: x.SanPhamId,
      khoId: this.khoId,
      soLuong: x.SoLuong
    }));

    await this.http.put('https://localhost:7161/api/products/cap-nhat-so-luong', updateKhoPayload).toPromise();
    notify('Cập nhật tồn kho thành công', 'success', 2000);

    this.nhaCungCapId = '';
    this.chiTietPhieuNhap = [];
    this.tongTien = 0;
    this.loadSanPhamByKhoId(this.khoId);

  } catch (err: any) {
    console.error(err);
    notify(`Lỗi: ${err?.message || 'Không thể lưu phiếu nhập hoặc cập nhật tồn kho'}`, 'error', 3000);
  }
}


  showDanhSach() {
    this.router.navigate(['/lich-su']);
  }


  /* ===== EXPORT ===== */
  onExporting(e: any) {
    const workbook = new ExcelJS.Workbook();
    const worksheet = workbook.addWorksheet('Phiếu nhập kho');

    worksheet.mergeCells('A1:E1');
    worksheet.getCell('A1').value = 'PHIẾU NHẬP KHO';
    worksheet.getCell('A1').font = { size: 16, bold: true };
    worksheet.getCell('A1').alignment = { horizontal: 'center' };

    const ncc = this.nhaCungCapDs.find(
      x => x.nhaCungCapId === this.nhaCungCapId
    );

    worksheet.addRow([]);
    worksheet.addRow(['Nhà cung cấp:', ncc?.tenNhaCungCap || '']);

    const startRow = worksheet.rowCount + 2;

    exportDataGrid({
      component: e.component,
      worksheet,
      topLeftCell: { row: startRow, column: 1 },
      autoFilterEnabled: true
    }).then(() => {
      const totalRow = worksheet.rowCount + 2;
      worksheet.mergeCells(`A${totalRow}:D${totalRow}`);
      worksheet.getCell(`A${totalRow}`).value = 'TỔNG TIỀN';
      worksheet.getCell(`A${totalRow}`).font = { bold: true };
      worksheet.getCell(`E${totalRow}`).value = this.tongTien;
      worksheet.getCell(`E${totalRow}`).numFmt = '#,##0';

      workbook.xlsx.writeBuffer().then(buffer => {
        saveAs(
          new Blob([buffer], { type: 'application/octet-stream' }),
          'Phieu_Nhap_Kho.xlsx'
        );
      });
    });

    e.cancel = true;
  }
  onKhoChanged(e: any) {
    this.loadSanPhamByKhoId(this.khoId);
  } 
}
