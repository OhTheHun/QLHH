import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import {
  DxButtonModule,
  DxDataGridModule,
  DxDateBoxModule,
  DxFormModule,
  DxSelectBoxModule,
  DxTextBoxModule
} from "devextreme-angular";
import { exportDataGrid } from "devextreme/excel_exporter";
import * as ExcelJS from "exceljs";
import { saveAs } from "file-saver";
import { HttpClient } from "@angular/common/http";
import notify from "devextreme/ui/notify"; 

export interface SanPham {
  sanPhamId: string;
  danhMucId: string;
  tenSanPham: string;
  hinhAnh?: string;
  donGia: number;
  trangThai: boolean;
}

export interface HistoryRow {
  id: string;
  sanPhamId: string;
  code: string;
  tenSanPham: string;
  soLuong: number;
  donGia: number;
  ngayGiao: string; 
  loai: 'Nhap' | 'Xuat';
}

@Component({
  selector: 'app-history',
  standalone: true,
  imports: [
    CommonModule,
    DxFormModule,
    DxDataGridModule,
    DxButtonModule,
    DxSelectBoxModule,
    DxTextBoxModule,
    DxDateBoxModule,
    HttpClientModule
  ],
  templateUrl: './History.component.html',
  styleUrls: ['./History.component.scss']
})
export class HistoryComponent implements OnInit {
    constructor(private http: HttpClient) {}
    private readonly apiSanPham = 'https://localhost:7161/api/products/SanPham';
    private readonly apiLichSu = 'https://localhost:7161/api/product/history';
    SanPhamList: SanPham[] = [];

  filter = {
  code: '',
  sanPhamId: '',
  fromDate: null as Date | null,
  toDate: null as Date | null
};


  sanPhamDs: SanPham[] = [];
  historyDs: HistoryRow[] = [];
  filteredDs: HistoryRow[] = [];

  ngOnInit(): void {
    this.loadSanPham();
    this.loadHistory();
  }


  loadSanPham() {
    this.http.get<SanPham[]>(this.apiSanPham).subscribe({
      next: (data) => {
        this.SanPhamList = data;
        console.log('Sản phẩm:', data);
      },
      error: (err) => {
        console.error(err);
        notify('Chạy localhost backend với mở api chưa ku?', 'error', 2000);
      }
    });
  }

  loadHistory() {
    this.http.get<HistoryRow[]>(this.apiLichSu).subscribe({
      next: (data) => {
        this.historyDs = data;
        this.filteredDs = [...data];
        console.log('Lịch sử:', data);
      },
      error: (err) => {
        console.error(err);
        notify('Chạy localhost backend với mở api chưa ku?', 'error', 2000);
      }
    });
  }

  search() {
  this.filteredDs = this.historyDs.filter(x => {

    const matchCode =
      !this.filter.code ||
      x.code.toLowerCase().includes(this.filter.code.toLowerCase());

    const matchSanPham =
  !this.filter.sanPhamId ||
  x.sanPhamId === this.filter.sanPhamId;


    const giaoDate = new Date(x.ngayGiao + 'T00:00:00');
    giaoDate.setHours(0, 0, 0, 0);

    const from = this.filter.fromDate ? new Date(this.filter.fromDate) : null;
    const to = this.filter.toDate ? new Date(this.filter.toDate) : null;

    from?.setHours(0, 0, 0, 0);
    to?.setHours(23, 59, 59, 999);

    const matchFrom = !from || giaoDate >= from;
    const matchTo = !to || giaoDate <= to;

    return matchCode && matchSanPham && matchFrom && matchTo;
  });
}

  exportExcel(e: any) {
    const workbook = new ExcelJS.Workbook();
    const worksheet = workbook.addWorksheet('Lịch sử nhập xuất');

    exportDataGrid({
      component: e.component,
      worksheet,
      autoFilterEnabled: true
    }).then(() => {
      workbook.xlsx.writeBuffer().then(buffer => {
        saveAs(
          new Blob([buffer], { type: 'application/octet-stream' }),
          'Lich_su_nhap_xuat.xlsx'
        );
      });
    });

    e.cancel = true;
  }

  calcThanhTien = (row: HistoryRow) =>
    row.soLuong * row.donGia;
}
