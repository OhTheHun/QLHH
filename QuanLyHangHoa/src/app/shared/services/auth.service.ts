import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { BehaviorSubject, lastValueFrom } from 'rxjs';
import { HttpClient } from '@angular/common/http';

/* ================== INTERFACES ================== */
export interface IUser {
  userId: string;
  email: string;
  avatarUrl?: string;
  role: 'ADMIN' | 'NHANVIEN' | '';
}

export interface ProfileUser {
  userId: string;
  email: string;
  role: string;
  fullName?: string;
  phoneNumber?: string;
  status?: string;
  avatarUrl?: string;
}

const defaultPath = '/home';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private userSubject = new BehaviorSubject<IUser | null>(null);
  user$ = this.userSubject.asObservable();

  private _user: IUser | null = null;
  private _profileUser: ProfileUser | null = null;

  private apiLogin = 'https://localhost:7161/api/person/login';
  private apiRegister = 'https://localhost:7161/api/person/register';

  private _lastAuthenticatedPath: string = defaultPath;

  constructor(
    private router: Router,
    private http: HttpClient
  ) {
    /* ===== Load user khi refresh ===== */
    const savedUser = localStorage.getItem('currentUser');
    if (savedUser && savedUser !== 'undefined' && savedUser !== 'null') {
      this._user = JSON.parse(savedUser);
      this.userSubject.next(this._user);
    }

    const savedProfile = localStorage.getItem('profileUser');
    if (savedProfile && savedProfile !== 'undefined' && savedProfile !== 'null') {
      this._profileUser = JSON.parse(savedProfile);
    }

    const lastPath = localStorage.getItem('lastPath');
    if (lastPath) {
      this._lastAuthenticatedPath = lastPath;
    }
  }

  /* ===== GETTERS ===== */
  get loggedIn(): boolean {
    return !!this._user;
  }

  get user(): IUser | null {
    return this._user;
  }

  get role(): string {
    return this._user?.role ?? '';
  }

  get profileUser(): ProfileUser | null {
    return this._profileUser;
  }

  get lastAuthenticatedPath(): string {
    return this._lastAuthenticatedPath;
  }

  set lastAuthenticatedPath(value: string) {
    this._lastAuthenticatedPath = value;
    localStorage.setItem('lastPath', value);
  }

  /* ===== LOGIN ===== */
  async logIn(
    email: string,
    password: string
  ): Promise<{ isOk: boolean; data?: IUser; message?: string }> {
    try {
      const res: any = await lastValueFrom(
        this.http.post(this.apiLogin, { email, password })
      );

      if (!res || !res.userId) {
        return { isOk: false, message: 'Đăng nhập thất bại' };
      }

      // Lưu profileUser
      this._profileUser = {
        userId: res.userId,
        email: res.email ?? email,
        role: res.role?.trim() as 'ADMIN' | 'NHANVIEN' || 'NHANVIEN',
        fullName: res.fullName,
        phoneNumber: res.phoneNumber,
        avatarUrl: res.hinhAnh
      };
      localStorage.setItem('profileUser', JSON.stringify(this._profileUser));

      // Lưu user
      this._user = {
        userId: res.userId,
        email: res.email ?? email,
        avatarUrl: res.hinhAnh || `https://ui-avatars.com/api/?name=${email}`,
        role: res.role?.trim() as 'ADMIN' | 'NHANVIEN' || 'NHANVIEN'
      };
      localStorage.setItem('currentUser', JSON.stringify(this._user));
      this.userSubject.next(this._user);

      // Navigate đến last path
      await this.router.navigate([this._lastAuthenticatedPath], { replaceUrl: true });

      return { isOk: true, data: this._user };

    } catch (err: any) {
      return {
        isOk: false,
        message: err?.error?.message || 'Tài khoản sai hoặc không tồn tại'
      };
    }
  }

  async getUser(): Promise<{ isOk: boolean; data: IUser | null }> {
    return { isOk: true, data: this._user };
  }

  async createAccount(email: string, password: string) {
    try {
      this.router.navigate(['/login-form'], { replaceUrl: true });
      return { isOk: true };
    } catch {
      return { isOk: false, message: 'Failed to create account' };
    }
  }

  async changePassword(email: string, recoveryCode: string) {
    return { isOk: true, message: 'Password changed successfully' };
  }

  async resetPassword(email: string) {
    return { isOk: true, message: 'Reset password link sent to your email' };
  }

  /* ===== LOGOUT ===== */
  async logOut() {
    this._user = null;
    this._profileUser = null;
    this.userSubject.next(null);

    localStorage.removeItem('currentUser');
    localStorage.removeItem('profileUser');
    localStorage.removeItem('lastPath');
    this._lastAuthenticatedPath = defaultPath;

    // Navigate đến login-form và reset navigation
    await this.router.navigateByUrl('/login-form');
  }
}

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {

    const isLoggedIn = this.authService.loggedIn;

    const authPages = [
      'login-form',
      'reset-password',
      'create-account',
      'change-password/:recoveryCode'
    ];

    const isAuthPage = authPages.includes(
      route.routeConfig?.path || ''
    );

    if (!isLoggedIn && !isAuthPage) {
      this.router.navigate(['/login-form']);
      return false;
    }

    if (isLoggedIn && isAuthPage) {
      this.router.navigate(['/home']);
      return false;
    }

    return true;
  }
}
