import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpClientModule, HttpParams } from '@angular/common/http';
import { DxChartModule, DxDataGridModule } from 'devextreme-angular';

@Component({
  templateUrl: 'home.component.html',
  styleUrls: ['./home.component.scss'],
  standalone: true,
  imports: [DxDataGridModule, DxChartModule, CommonModule, HttpClientModule]
})
export class HomeComponent implements OnInit {

  summaryCards: any[] = [];
  chartTopProducts: any[] = [];
  lowStockProducts: any[] = [];

  selectedMonth: Date = new Date(); // mặc định tháng hiện tại

  private summaryApi = "https://localhost:7161/api/products/summary";
  private topProductsApi = "https://localhost:7161/api/products/top-products";
  private lowStockApi = "https://localhost:7161/api/products/low-stock";

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadSummary();
    this.loadTopProducts();
    this.loadLowStock();
  }

  private formatMonth(date: Date): string {
  return date.toISOString().split('T')[0]; // yyyy-MM-dd
}

  loadSummary(): void {
    const params = new HttpParams().set('month', this.formatMonth(this.selectedMonth));

    this.http.get<any>(this.summaryApi, { params }).subscribe({
      next: res => {
        this.summaryCards = [
          { title: "Tổng sản phẩm", value: res.tongSanPham },
          { title: "Tổng nhập tháng", value: res.tongNhapHang },
          { title: "Tổng xuất tháng", value: res.tongXuatHang },
          { title: "Doanh thu tháng", value: res.tongDoanhThu }
        ];
      },
      error: err => {
        console.error("Lỗi load summary:", err);
      }
    });
  }

  loadTopProducts(): void {
    this.http.get<any[]>(this.topProductsApi).subscribe({
      next: res => {
        this.chartTopProducts = res.map(x => ({
          SanPham: x.tenSanPham,
          SoLuong: x.soLuong
        }));
      },
      error: err => {
        console.error("Lỗi load top products:", err);
      }
    });
  }

  loadLowStock(): void {
    this.http.get<any[]>(this.lowStockApi).subscribe({
      next: res => {
        this.lowStockProducts = res.map(x => ({
          SanPham: x.tenSanPham,
          TonKho: x.tonKho
        }));
      },
      error: err => {
        console.error("Lỗi load low stock:", err);
      }
    });
  }
}
