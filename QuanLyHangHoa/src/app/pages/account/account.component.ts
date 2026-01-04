import { Component, OnInit } from '@angular/core';
import { CommonModule } from "@angular/common";
import {
  DxButtonModule,
  DxDataGridModule,
  DxTemplateModule,
  DxFormModule,
  DxSelectBoxModule
} from "devextreme-angular";
import { HttpClient, HttpClientModule } from '@angular/common/http';
import notify from 'devextreme/ui/notify';

@Component({
  selector: 'app-tai-khoan',
  templateUrl: 'account.component.html',
  styleUrls: ['./account.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    DxFormModule,
    DxDataGridModule,
    DxButtonModule,
    DxTemplateModule,
    DxSelectBoxModule,
    HttpClientModule
  ]
})
export class AccountComponent implements OnInit {

  private apiUrl = 'https://localhost:7161/api/person/account';

  users: any[] = [];

  vaiTro = [
    { value: 'ADMIN' },
    { value: 'NHANVIEN' }
  ];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadAccounts();
  }

  loadAccounts() {
    this.http.get<any[]>(this.apiUrl).subscribe({
      next: (res) => {
        this.users = res;
        console.log('Danh sách tài khoản:', res);
      },
      error: (err) => {
        console.error('Lỗi load account', err);
        notify(err.error?.error || 'Lỗi tải danh sách tài khoản');
      }
    });
  }

  onRowInserted(e: any) {
    this.http.post<any>(this.apiUrl, e.data).subscribe({
      next: () => {
        this.loadAccounts();
      },
      error: (err) => {
        notify(err.error?.error || 'Lỗi thêm tài khoản');
        this.loadAccounts();
      }
    });
  }
}
