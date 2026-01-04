import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HttpClient, HttpClientModule } from "@angular/common/http";
import {
  DxButtonModule,
  DxChartModule,
  DxDataGridModule,
  DxDateBoxModule
} from "devextreme-angular";
import { DxiSeriesModule } from "devextreme-angular/ui/nested";

@Component({
  selector: 'app-report',
  templateUrl: './Report.component.html',
  styleUrls: ['./Report.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    HttpClientModule,
    DxDataGridModule,
    DxDateBoxModule,
    DxChartModule,
    DxButtonModule,
    DxiSeriesModule
  ]
})
export class ReportComponent implements OnInit {

  fromDate!: Date;
  toDate!: Date;

  errorMessage = "";
  loading = false;

  dataReport: any[] = [];
  chartData: any[] = [];

  private apiUrl = "https://localhost:7161/api/product/report";

  constructor(private http: HttpClient) {}

  //lifeCycleHook, gọi 1 lần tự động 
  ngOnInit(): void {
    const today = new Date();
    //constructor ngày tháng mặc định 
    this.toDate = today;
    this.fromDate = new Date(today.getFullYear(), today.getMonth(), 1); 
  }

  private formatDate(date: Date): string {
    return date.toISOString().split('T')[0]; // chuyển date thành chuỗi định dạng yyyy-MM-dd
  }

  validateRange(): boolean {
    if (!this.fromDate || !this.toDate) {
      this.errorMessage = "Vui lòng chọn đầy đủ Từ ngày và Đến ngày.";
      return false;
    }

    if (this.fromDate > this.toDate) {
      this.errorMessage = "Từ ngày phải nhỏ hơn hoặc bằng Đến ngày.";
      return false;
    }

    this.errorMessage = "";
    return true;
  }

  loadData(): void {
    if (!this.validateRange()) return;

    this.loading = true;

    const params = {
      fromDate: this.formatDate(this.fromDate),
      toDate: this.formatDate(this.toDate)
    };

    this.http.get<any[]>(this.apiUrl, { params }).subscribe({
      next: (res) => {
        this.dataReport = res;

        this.chartData = res.map(item => ({
          SanPham: item.tenSanPham,
          Nhap: item.tongNhap,
          Xuat: item.tongXuat,
          DoanhThu: item.doanhThu
        }));

        this.loading = false;
      },
      error: () => {
        this.errorMessage = "Không thể tải dữ liệu báo cáo.";
        this.loading = false;
      }
    });
  }
}
