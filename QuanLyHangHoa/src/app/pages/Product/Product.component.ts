import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { DxButtonModule, DxDataGridModule } from "devextreme-angular";
import notify from 'devextreme/ui/notify';
import { confirm } from "devextreme/ui/dialog";
import { HttpClient, HttpClientModule } from "@angular/common/http";
import { Observable } from "rxjs";



export interface SanPham {
  sanPhamId: string;
  danhMucId: string;
  tenSanPham: string;
  hinhAnh?: string;
  donGia: number;
  trangThai: boolean;
}

export interface DanhMucSanPham {
  danhMucId: string;
  tenDanhMuc: string;
}

@Component({
  selector: 'app-product',
  templateUrl: './Product.component.html',
  styleUrls: ['./Product.component.scss'],
  standalone: true,
  imports: [DxDataGridModule, DxButtonModule, CommonModule, HttpClientModule],
})
export class ProductComponent {
  constructor(private http: HttpClient) {}
  
  private readonly apiDanhMuc = 'https://localhost:7161/api/products/DanhMuc';
  danhMucList: DanhMucSanPham[] = [];
  private readonly apiSanPham = 'https://localhost:7161/api/products/SanPham';
  SanPhamList: SanPham[] = [];
  
  ngOnInit(): void {
    this.loadDanhMuc();
    this.loadSanPham();
  }
  getDanhSacSanPham(): Observable<SanPham[]> {
    return this.http.get<SanPham[]>(this.apiSanPham);
  }

  loadDanhMuc() {
    this.http.get<DanhMucSanPham[]>(this.apiDanhMuc).subscribe({
      next: (data) => {
        this.danhMucList = data;
        console.log('Danh muc:', data);
      },
      error: (err) => {
        console.error(err);
        notify('Không lấy được danh sách danh muc', 'error', 2000);
      }
    });
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



  selectedKeys: string[] = [];


  trangThaiOptions = [
    { value: true, text: 'Hoạt động' },
    { value: false, text: 'Ngừng bán' }
  ];

  onSelectionChanged(e: any) {
    this.selectedKeys = e.selectedRowKeys;
  }

  deleteSelected() {
  if (!this.selectedKeys.length) {
    notify('Chưa chọn dòng nào để xóa', 'error', 2000);
    return;
  }

  const result = confirm(
    'Bạn có chắc muốn xóa các mục đã chọn?',
    'Xác nhận xóa'
  );

  result.then((dialogResult) => {
    if (dialogResult) {
      this.SanPhamList = this.SanPhamList.filter(
        p => !this.selectedKeys.includes(p.sanPhamId)
      );
      this.selectedKeys = [];
      notify('Xóa thành công', 'success', 2000);
    } else {
      notify('Hủy xóa', 'error', 1500);
    }
  });
}

}

