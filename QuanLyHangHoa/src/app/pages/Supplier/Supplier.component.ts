import { Component } from "@angular/core";
import { DxDataGridModule } from "devextreme-angular";
import notify from "devextreme/ui/notify";
import { HttpClient, HttpClientModule } from "@angular/common/http";
import { Observable } from "rxjs";
export interface NhaCungCap {
  nhaCungCapId: string;
  tenNhaCungCap: string;
  diaChi: string;
  email: string;
  sdt: string;
} 

@Component({
  selector: 'app-supplier',
  templateUrl: './Supplier.component.html',
  styleUrls: ['./Supplier.component.scss'],
  standalone: true,
  imports: [DxDataGridModule, HttpClientModule]
})
export class SupplierComponent {
constructor(private http: HttpClient) {}
 private readonly apiUrl =
    'https://localhost:7161/api/person/Nhacungcap';

  nhaCungCapDs: NhaCungCap[] = [];

  getDanhSachNhaCungCap(): Observable<NhaCungCap[]> {
    return this.http.get<NhaCungCap[]>(this.apiUrl);
  }

  ngOnInit(): void {
    this.loadNhaCungCap();
  }

  loadNhaCungCap() {
    this.http.get<NhaCungCap[]>(this.apiUrl).subscribe({
      next: (data) => {
        this.nhaCungCapDs = data;
        console.log('NhaCungCap:', data);
      },
      error: (err) => {
        console.error(err);
        notify('Không lấy được danh sách nhà cung cấp', 'error', 2000);
      }
    });
  }


  onRowInserting(e: any) {
    if (this.isDuplicateName(e.data.TenNhaCungCap)) {
      notify('Tên nhà cung cấp đã tồn tại!', 'error', 2000);
      e.cancel = true;
    }
  }

  onRowUpdating(e: any) {
    if (e.newData.TenNhaCungCap && this.isDuplicateName(e.newData.TenNhaCungCap, e.oldData.NhaCungCapId)) {
      notify('Tên nhà cung cấp đã tồn tại!', 'error', 2000);
      e.cancel = true;
    }
  }
  private isDuplicateName(name: string, idToExclude?: string): boolean {
    return this.nhaCungCapDs.some(ncc =>
      ncc.tenNhaCungCap === name && ncc.nhaCungCapId !== idToExclude
    );
  }
  onRowRemoving(e: any) {
    notify(`Đã xóa nhà cung cấp: ${e.data.TenNhaCungCap}`, 'success', 2000);
  }

}
