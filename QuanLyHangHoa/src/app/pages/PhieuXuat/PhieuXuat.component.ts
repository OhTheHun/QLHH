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

/* ===== INTERFACES ===== */
export interface KhachHang {
  khachHangId: string;
  tenKH: string;
  diaChi?: string;
  email?: string;
  sdt?: string;
}
export interface Kho {
  khoId: string;
  tenKho: string;
  diaChi?: string;
}
export interface SanPham {
  sanPhamId: string;
  danhMucId: string;
  tenSanPham: string;
  hinhAnh?: string;
  donGia: number;
  trangThai: boolean;
}
interface ChiTietPhieuXuat {
  SanPhamId: string;
  SoLuong: number;
  DonGia: number;
  NgayGiaoHang: Date;
}
interface SanPhamByKhoId {
  sanPhamId: string;
  tenSanPham: string;
  soLuong: number;
}

@Component({
  selector: 'app-phieu-xuat',
  standalone: true,
  imports: [
    CommonModule,
    HttpClientModule,
    DxDataGridModule,
    DxButtonModule,
    DxSelectBoxModule,
    DxDateBoxModule
  ],
  templateUrl: './PhieuXuat.component.html',
  styleUrls: ['./PhieuXuat.component.scss']
})
export class PhieuXuatComponent implements OnInit {

  /* ===== API ===== */
  private apiKhachHang = 'https://localhost:7161/api/person/KhachHang';
  private apiSanPham = 'https://localhost:7161/api/products/SanPham';
  private apiCreatePhieuXuat = 'https://localhost:7161/api/products/phieu-xuat';
  private apiKho = 'https://localhost:7161/api/products/Kho';
  private apiSanPhamByKhoId = 'https://localhost:7161/api/products/product-byKhoId';

  constructor(
    private router: Router,
    private authService: AuthService,
    private http: HttpClient
  ) {}

  today = new Date();

  khachHangId = '';
  khoId = '';
  sanPhamId = '';
  khachHangDs: KhachHang[] = [];
  khoDs: Kho[] = [];
  sanPhamDs: SanPham[] = [];
  sanPhamByKhoDs: SanPhamByKhoId[] = [];
  chiTietPhieuXuat: ChiTietPhieuXuat[] = [];
  tongTien = 0;

  ngOnInit(): void {
    const user = this.authService.user;
    if (!user?.userId) {
      notify('Chưa đăng nhập', 'error', 2000);
      return;
    }

    this.loadKhachHang();
    this.loadSanPham();
    this.loadKho();
    this.loadSanPhamByKhoId(this.khoId);
  }

  /* ===== LOAD KHÁCH HÀNG ===== */
  loadKhachHang() {
    this.http.get<KhachHang[]>(this.apiKhachHang).subscribe({
      next: res => this.khachHangDs = res,
      error: () => notify('Không load được khách hàng', 'error', 2000)
    });
  }

  /* ===== LOAD SẢN PHẨM ===== */
  loadSanPham() {
    this.http.get<SanPham[]>(this.apiSanPham).subscribe({
      next: res => this.sanPhamDs = res,
      error: () => notify('Không load được sản phẩm', 'error', 2000)
    });
  }

  loadKho() {
    this.http.get<Kho[]>(this.apiKho).subscribe({
      next: res => this.khoDs = res,
      error: () => notify('Không load được kho', 'error', 2000)
    });
  }

  loadSanPhamByKhoId(khoId: string) {
    if (!khoId) {
      this.sanPhamByKhoDs = [];
      return;
    }
    this.http.get<SanPhamByKhoId[]>(`${this.apiSanPhamByKhoId}?khoId=${khoId}`).subscribe({
      next: res => this.sanPhamByKhoDs = res,
      error: () => notify('Không load được sản phẩm theo kho', 'error', 2000)
    });
  }

  getSoLuongTon(sanPhamId: string): number {
    const sp = this.sanPhamByKhoDs.find(x => x.sanPhamId === sanPhamId);
    return sp ? sp.soLuong : 0;
  }

  /* ===== TÍNH TIỀN ===== */
  calcThanhTien = (row: ChiTietPhieuXuat) => (row.SoLuong || 0) * (row.DonGia || 0);

  recalcTotal() {
    this.tongTien = this.chiTietPhieuXuat.reduce((s, x) => s + this.calcThanhTien(x), 0);
  }

  /* ===== SAVE PHIẾU XUẤT ===== */
  async savePhieuXuat() {
    const user = this.authService.user;
    if (!user?.userId) {
      notify('Chưa đăng nhập', 'error', 2000);
      return;
    }

    if (!this.khachHangId) {
      notify('Chưa chọn khách hàng', 'error', 2000);
      return;
    }

    if (!this.chiTietPhieuXuat.length) {
      notify('Phiếu xuất chưa có sản phẩm', 'error', 2000);
      return;
    }

    const invalid = this.chiTietPhieuXuat.some(
      x => !x.SanPhamId || !x.SoLuong || x.SoLuong <= 0 || x.SoLuong > this.getSoLuongTon(x.SanPhamId)
    );
    if (invalid) {
      notify('Chi tiết phiếu xuất không hợp lệ hoặc chưa có api', 'error', 3000);
      return;
    }

    const ok = await confirm('Xác nhận xuất kho?', 'Xác nhận');
    if (!ok) return;

    const payload = {
      userId: user.userId,
      khachHangId: this.khachHangId,
      chiTietPhieuXuat: this.chiTietPhieuXuat.map(x => ({
        sanPhamId: x.SanPhamId,
        soLuong: x.SoLuong,
        donGia: x.DonGia,
        ngayGiaoHang: x.NgayGiaoHang
      }))
    };

    try {
      await this.http.post(this.apiCreatePhieuXuat, payload).toPromise();
      notify('Lưu phiếu xuất thành công', 'success', 2000);

      const updateKhoPayload = this.chiTietPhieuXuat.map(x => ({
        sanPhamId: x.SanPhamId,
        khoId: this.khoId,
        soLuong: -x.SoLuong // trừ tồn kho
      }));
      await this.http.put('https://localhost:7161/api/products/cap-nhat-so-luong', updateKhoPayload).toPromise();
      notify('Cập nhật tồn kho thành công', 'success', 2000);

      this.khachHangId = '';
      this.chiTietPhieuXuat = [];
      this.tongTien = 0;
      this.loadSanPhamByKhoId(this.khoId);

    } catch (err: any) {
      console.error(err);
      notify(`Lỗi: ${err?.message || 'Không thể lưu phiếu xuất hoặc cập nhật tồn kho'}`, 'error', 3000);
    }
  }

  showDanhSach() {
    this.router.navigate(['/lich-su']);
  }

  /* ===== EXPORT ===== */
  onExporting(e: any) {
    const workbook = new ExcelJS.Workbook();
    const worksheet = workbook.addWorksheet('Phiếu xuất kho');

    worksheet.mergeCells('A1:E1');
    worksheet.getCell('A1').value = 'PHIẾU XUẤT KHO';
    worksheet.getCell('A1').font = { size: 16, bold: true };
    worksheet.getCell('A1').alignment = { horizontal: 'center' };

    const kh = this.khachHangDs.find(x => x.khachHangId === this.khachHangId);

    worksheet.addRow([]);
    worksheet.addRow(['Khách hàng:', kh?.tenKH || '']);

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
          'Phieu_Xuat_Kho.xlsx'
        );
      });
    });

    e.cancel = true;
  }

  onKhoChanged(e: any) {
    this.loadSanPhamByKhoId(this.khoId);
  }

  onRowValidating(e: any) {
    const data = { ...e.oldData, ...e.newData };

    if (!data.SanPhamId || !data.SoLuong) return;

    const tonKho = this.getSoLuongTon(data.SanPhamId);

    if (data.SoLuong > tonKho) {
      e.isValid = false;
      e.errorText = `Số lượng xuất (${data.SoLuong}) vượt tồn kho (${tonKho})`;
    }
  }

}
