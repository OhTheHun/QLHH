export type Role = 'ADMIN' | 'NHANVIEN' | ' ';


export const navigation = [
  {
    text: 'Trang chủ',
    path: '/home',
    icon: 'home',
    roles: ['ADMIN', 'NHANVIEN',]
  },
  {
    text: 'Quản lý kho',
    icon: 'box',
    roles: ['ADMIN', 'NHANVIEN',],
    items: [
      { text: 'Kho hàng', path: '/kho-hang', roles: ['ADMIN', 'NHANVIEN',] },
      { text: 'Hàng hóa', path: '/hang-hoa', roles: ['ADMIN', 'NHANVIEN',] },
      { text: 'Xuất phiếu', path: '/xuat-phieu', roles: ['ADMIN', 'NHANVIEN',] },
      { text: 'Nhập phiếu', path: '/nhap-phieu', roles: ['ADMIN', 'NHANVIEN',] },
      { text: 'Lịch sử', path: '/lich-su', roles: ['ADMIN', 'NHANVIEN',] }
    ]
  },
  {
    text: 'Đối tác',
    icon: 'tel',
    roles: ['ADMIN'],
    items: [
      {
        text: 'Khách hàng',
        path: '/khach-hang',
        roles: ['ADMIN'],
      },
      {
        text: 'Nhà cung cấp',
        path: '/nha-cung-cap',
        roles: ['ADMIN'],
      }
    ]
  },
  {
    text: 'Báo cáo',
    path: '/bao-cao',
    icon: 'chart',
    roles: ['ADMIN'],
  },
  {
    text: 'Cài đặt',
    path: '/settings',
    icon: 'preferences',
    roles: ['ADMIN'],
    items: [
      { text: 'Tài Khoản', path: '/settings/users', roles: ['ADMIN'], },
    ]
  },

];
