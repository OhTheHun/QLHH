import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DxButtonModule } from 'devextreme-angular/ui/button';
import { DxTextBoxModule } from 'devextreme-angular/ui/text-box';
import notify from 'devextreme/ui/notify';
import { confirm } from 'devextreme/ui/dialog';
import { HttpClient } from '@angular/common/http';
import { AuthService, IUser } from '../../shared/services';
import { DxValidationGroupModule, DxValidatorModule } from 'devextreme-angular';

export interface IUserProfile {
  userId: string;
  email: string;
  tenUser: string;
  diaChi: string;
  sdt: string;
  hinhAnh: string;
  note: string;
  role: string;
}

@Component({
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    DxButtonModule,
    DxTextBoxModule,
    DxValidatorModule,
    DxValidationGroupModule
  ],
})
export class ProfileComponent implements OnInit {

  imageUrlPattern = /^https?:\/\/.*\.(jpe?g|jpeg|png)$/i;
  isEditing = false;
  imageUrlInput = '';

  employee: IUserProfile = {
    userId: '',
    email: '',
    tenUser: '',
    diaChi: '',
    sdt: '',
    hinhAnh: '',
    note: '',
    role: ''
  };

  originalEmployee: IUserProfile = {} as IUserProfile;
  private apiUrl = 'https://localhost:7161/api/person/'; 

  constructor(private http: HttpClient, private authService: AuthService) { }

  ngOnInit(): void {
    const currentUser: IUser | null = this.authService.user;
    if (currentUser?.userId) {
      this.loadProfile(currentUser.userId);
    } else {
      notify("Chưa có thông tin đăng nhập!", "error", 2000);
    }
  }

  async loadProfile(userId: string) {
    try {
      const res = await this.http.get<IUserProfile>(`${this.apiUrl}${userId}`).toPromise();
      if (res) {
        this.employee = res;
        this.imageUrlInput = res.hinhAnh || '';
      } else {
        notify("Không tìm thấy thông tin người dùng", "error", 2000);
      }
    } catch (err) {
      notify("Không thể tải thông tin người dùng", "error", 2000);
    }
  }

  toggleEdit() {
    if (!this.isEditing) {
      this.originalEmployee = { ...this.employee };
      this.imageUrlInput = this.employee.hinhAnh;
      this.isEditing = true;
      return;
    }

    this.employee.hinhAnh = this.imageUrlInput.trim() || '';

    confirm("Bạn có chắc chắn muốn lưu thay đổi?", "Xác nhận")
      .then(async (ok: boolean) => {
        if (!ok) {
          this.employee = { ...this.originalEmployee };
          this.imageUrlInput = this.employee.hinhAnh;
          this.isEditing = false;
          notify("Đã hủy thay đổi!", "error", 2000);
          return;
        }

        await this.saveProfile();
      });
  }

  async saveProfile() {
    try {
      const payload = {
        tenUser: this.employee.tenUser,
        diaChi: this.employee.diaChi,
        sdt: this.employee.sdt,
        hinhAnh: this.employee.hinhAnh,
        note: this.employee.note
      };

      await this.http.put(`${this.apiUrl}${this.employee.userId}`, payload).toPromise();
      this.originalEmployee = { ...this.employee };
      this.isEditing = false;
      notify("Lưu thông tin thành công!", "success", 2000);
    } catch (err: any) {
      const msg = err?.error?.error || err?.message || "Lưu thông tin thất bại!";
      notify(msg, "error", 2000);
    }
  }

  onImageUrlChanged(e: any) {
    const url = e.value?.trim() || '';
    this.imageUrlInput = url;
    this.employee.hinhAnh = this.imageUrlPattern.test(url) ? url : '';
    if (url && !this.imageUrlPattern.test(url)) {
      notify("URL ảnh không hợp lệ! Chỉ chấp nhận .jpg, .jpeg, .png", "error", 2000);
    }
  }

  removeImage() {
    this.imageUrlInput = '';
    this.employee.hinhAnh = '';
  }
}
