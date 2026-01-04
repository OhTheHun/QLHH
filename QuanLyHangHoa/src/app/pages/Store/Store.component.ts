import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { DxButtonModule, DxDataGridModule } from 'devextreme-angular';
import notify from 'devextreme/ui/notify';
import { HttpClient, HttpClientModule } from '@angular/common/http';

export interface SanPham {
  sanPhamId: string;
  danhMucId: string;
  tenSanPham: string;
  hinhAnh?: string;
  donGia: number;
  trangThai: boolean;
}

interface Kho {
  khoId: string;
  tenKho: string;
  diaChi: string;
}

interface ChiTietKho {
  sanPhamId: string;
  tenSanPham: string;
  khoId: string; 
  tenKho: string;      
  soLuong: number;
}

@Component({
  selector: 'app-store',
  templateUrl: './Store.component.html',
  styleUrls: ['./Store.component.scss'],
  standalone: true,
  imports: [CommonModule, DxDataGridModule, DxButtonModule, HttpClientModule],
})
export class StoreComponent {
  constructor(private http: HttpClient) {}

  khoList: Kho[] = [];
  chiTietKhoList: ChiTietKho[] = [];
  SanPhamList: SanPham[] = [];
  selectedRows: ChiTietKho[] = [];

  private readonly apiKho = "https://localhost:7161/api/products/Kho";
  private readonly apiChiTietKho = "https://localhost:7161/api/products/chi-tiet-kho";
  private readonly apiSanPham = 'https://localhost:7161/api/products/SanPham';

  ngOnInit(): void {
    this.loadKho();
    this.loadChiTietKho();
    this.loadSanPham();
  }

  loadKho() {
    this.http.get<Kho[]>(this.apiKho).subscribe({
      next: (data) => this.khoList = data,
      error: () => notify('Không lấy được danh sách kho', 'error', 2000)
    });
  }

  loadChiTietKho() {
    this.http.get<ChiTietKho[]>(this.apiChiTietKho).subscribe({
      next: (data) => this.chiTietKhoList = data,
      error: () => notify('Không lấy được danh sách chi tiết kho', 'error', 2000)
    });
  }

  loadSanPham() {
    this.http.get<SanPham[]>(this.apiSanPham).subscribe({
      next: (data) => this.SanPhamList = data,
      error: () => notify('Không lấy được danh sách sản phẩm', 'error', 2000)
    });
  }

  onSelectionChanged(e: any) {
    this.selectedRows = e.selectedRowsData;
  }

  getDiaChiFromRow = (rowData: ChiTietKho) => {
    if (!rowData.khoId) return 'Chưa chọn kho';
    const kho = this.khoList.find(k => k.khoId === rowData.khoId);
    return kho ? kho.diaChi : '';
  };

 onRowInserted(e: any) {
  const row: ChiTietKho = e.data;

  if (!row.khoId || !row.sanPhamId) {
    notify('Vui lòng chọn Kho và Sản phẩm', 'error', 2000);
    return;
  }

  const payload = {
    sanPhamId: row.sanPhamId,
    khoId: row.khoId,
    soLuong: row.soLuong
  };

  this.http.post(this.apiChiTietKho, payload).subscribe({
    next: () => {
      notify('Thêm chi tiết kho thành công', 'success', 2000);
      this.loadChiTietKho(); // reload để key đúng
    },
    
  });
}

onRowUpdated(e: any) {
  const row: ChiTietKho = e.data;

  const payload = {
    sanPhamId: row.sanPhamId,
    khoId: row.khoId,
    soLuong: row.soLuong
  };

  this.http.put(`${this.apiChiTietKho}`, payload).subscribe({
    next: () => notify('Cập nhật thành công', 'success', 2000),
    error: () => notify('Cập nhật thất bại', 'error', 2000)
  });
}


  onRowRemoved(e: any) {
    const row: ChiTietKho = e.data;
    this.http.delete(`${this.apiChiTietKho}/${row.khoId}/${row.sanPhamId}`).subscribe({
      next: () => notify('Xóa thành công', 'success', 2000),
      error: () => notify('Xóa thất bại')
    });
  }
}
