# Tai Khoan Mock

File này liệt kê các tài khoản mock đang được seed trong `DbInitializer` để bạn test nhanh đăng nhập và phân quyền.

| Username | Email | Password | Role | Ghi chú |
| --- | --- | --- | --- | --- |
| admin | admin@system.com | Admin@123 | Admin | Tài khoản quản trị hệ thống |
| company1 | company1@system.com | Company@123 | Company | Doanh nghiệp ABC |
| company2 | company2@system.com | Company@123 | Company | Doanh nghiệp XYZ |
| student1 | student1@system.com | Student@123 | Student | Sinh viên Nguyễn Văn A |
| student2 | student2@system.com | Student@123 | Student | Sinh viên Trần Thị B |
| staff1 | staff1@system.com | Staff@123 | Officer | Cán bộ hệ thống |

## Luu y

- Các tài khoản này chỉ là dữ liệu mẫu để test.
- Lần đầu chạy ứng dụng, hệ thống sẽ tự tạo role và user nếu database đang trống.
- `staff1` hiện được gán vào role `Officer`.