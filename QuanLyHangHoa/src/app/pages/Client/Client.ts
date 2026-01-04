import { Component } from "@angular/core";
import { DxDataGridModule } from "devextreme-angular";
import notify from "devextreme/ui/notify";
import { HttpClient, HttpClientModule } from "@angular/common/http";

export interface KhachHang {
  khachHangId: string;
  tenKH: string;
  diaChi: string;
  email: string;
  sdt: string;
}

@Component({
  selector: 'app-customer',
  templateUrl: './Client.component.html',
  styleUrls: ['./Client.component.scss'],
  standalone: true,
  imports: [DxDataGridModule, HttpClientModule]
})
export class ClientComponent {

  constructor(private http: HttpClient) {}

  private readonly apiUrl =
    'https://localhost:7161/api/person/KhachHang';

  khachHangDs: KhachHang[] = [];

  ngOnInit(): void {
    this.loadKhachHang();
  }

  loadKhachHang() {
    this.http.get<KhachHang[]>(this.apiUrl).subscribe({
      next: (data) => {
        this.khachHangDs = data;
        console.log('KhachHang:', data);
      },
      error: (err) => {
        console.error(err);
        notify('Không lấy được danh sách khách hàng', 'error', 2000);
      }
    });
  }

  onRowInserting(e: any) {
    if (this.isDuplicateName(e.data.tenKH)) {
      notify('Tên khách hàng đã tồn tại!', 'error', 2000);
      e.cancel = true;
    }
  }

  onRowUpdating(e: any) {
    if (
      e.newData.tenKH &&
      this.isDuplicateName(e.newData.tenKH, e.oldData.khachHangId)
    ) {
      notify('Tên khách hàng đã tồn tại!', 'error', 2000);
      e.cancel = true;
    }
  }

  private isDuplicateName(
    name: string,
    idToExclude?: string
  ): boolean {
    return this.khachHangDs.some(kh =>
      kh.tenKH === name &&
      kh.khachHangId !== idToExclude
    );
  }

  onRowRemoving(e: any) {
    notify(`Đã xóa khách hàng: ${e.data.tenKH}`, 'success', 2000);
  }
}
