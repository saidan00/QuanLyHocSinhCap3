![hsms](logo.png)
###### Hệ thống quản lý Học sinh cấp 3

#### Thành viên:
- Lưu Minh Hoàng (3117410088)
- Võ Hoàng Huy (3117410103)
- Hứa Thị Ánh Ngân (3117410156)

----
## Cài đặt:
**1. .NET Core**
- Chạy **SQL Server**
- Cập nhật **connectionString** phù hợp với SQL Server
  - *HighSchoolManagerAPI/appsettings.json*: Thêm vào "ConnectionStrings"
    ```text
    "ConnectionStrings": {
      "stringname":
        "
        Server=127.0.0.1;
        Database=HighSchoolDb;
        User ID=USERNAME;
        Password=PASSWORD;
        MultipleActiveResultSets=true
        "
     }
    ```
  - *HighSchoolManagerAPI/Startup.cs*: Line 50 thay tên thành **stringname** đã đặt như trên
    ```cs
    Configuration.GetConnectionString("stringname")
    ```
- Cài đặt [**.NET Core 3.0**](https://dotnet.microsoft.com/download)
- Cài đặt **Entity Framework Core .NET Command-line Tools**
  ```sh
  dotnet tool install --global dotnet-ef
  ```
- Mở **Terminal** (PowerShell nếu là Windows) và đi đến thư mục src/HighSchoolManagerAPI
  ```
  cd ./QuanLyHocSinhCap3/src/HighSchoolManagerAPI
  ```
- Chạy câu lệnh sau để cập nhật database với các entity
  ```
  dotnet ef database update --context Infrastructure.Persistence.HighSchoolContext
  ```
- Build solution
  ```sh
  cd ../
  dotnet build
  ```

**2. React**
- Cài đặt [**Node.js**](https://nodejs.org/en/)
- Đi đến thư mục HighSchoolManagerFE
  ```text
  cd ./HighSchoolManagerFE
  ```
- Chạy lệnh sau để thực hiện cài đặt, cập nhật các package
  ```text
  npm install
  ```

----
## Thực thi:
- Chạy server **.NET Core** (Lần chạy đầu tiên khá lâu do phải seed data)
  ```sh
  cd ../HighSchoolManagerAPI
  dotnet run
  ```
- Chạy server **React**
  ```
  cd ../HighSchoolManagerFE
  npm start
  ```
- Truy cập bằng browser
  ```
  127.0.0.1:3000 (Không dùng localhost:3000)
  ```
- Đăng nhập bằng tài khoản mặc định
  ```
  username: admin
  password: 123456
  ```

----
###### README viết bởi Lưu Minh Hoàng
