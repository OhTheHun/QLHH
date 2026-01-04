import { Routes } from '@angular/router';
import { LoginFormComponent, ResetPasswordFormComponent, CreateAccountFormComponent, ChangePasswordFormComponent } from './shared/components';
import { AuthGuardService } from './shared/services';
import { HomeComponent } from './pages/home/home.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { StoreComponent } from './pages/Store/Store.component';
import { ProductComponent } from './pages/Product/Product.component';
import { PhieuXuatComponent } from './pages/PhieuXuat/PhieuXuat.component';
import { PhieuNhapComponent } from './pages/PhieuNhap/PhieuNhap.component';
import { HistoryComponent } from './shared/components/History/History.component';
import { SupplierComponent } from './pages/Supplier/Supplier.component';
import { ClientComponent } from './pages/Client/Client';
import { ReportComponent } from './pages/Report/Report.component';
import { AccountComponent } from './pages/account/account.component';

export const routes: Routes = [
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: 'login-form',
    component: LoginFormComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: 'reset-password',
    component: ResetPasswordFormComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: 'create-account',
    component: CreateAccountFormComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: 'change-password/:recoveryCode',
    component: ChangePasswordFormComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: 'kho-hang',
    component: StoreComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: 'hang-hoa',
    component: ProductComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: 'xuat-phieu',
    component: PhieuXuatComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: 'nhap-phieu',
    component: PhieuNhapComponent,
    canActivate: [ AuthGuardService ]
  },
  {path: 'lich-su',
   component: HistoryComponent,
   canActivate: [ AuthGuardService ] 
  },
  {
    path: 'nha-cung-cap',
    component: SupplierComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: 'khach-hang',
    component: ClientComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: 'bao-cao',
    component: ReportComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: 'settings/users',
    component:  AccountComponent,
    canActivate: [ AuthGuardService ],
  },
  {
    path: '**',
    redirectTo: 'home'
  }
];
