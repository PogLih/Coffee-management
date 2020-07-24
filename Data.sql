create database Cafe
go
use Cafe
go
--Table
create table Tablecoffe
(
	id int identity primary key,
	name nvarchar(100)NOT NULL default N'chưa đặt tên',
	[status] int not null default 0 -- trống || có người
)
go
--Account
create table Account
(
	Username nvarchar(100) primary key,
	DisplayName nvarchar(100)NOT NULL default N'Nhân Viên',
	[PassWord] nvarchar(1000)NOT NULL default 0,
	[Type] int NOT NULL  default 0--1:chủ && 0:NV
)
go
--Category
create table Category
(
	id int identity primary key,
	name nvarchar(100)NOT NULL default N'chưa đặt tên',
)
go
--Drinks&somethingelse
create table Drinks
(
	id int identity primary key,
	name nvarchar(100)NOT NULL default N'Chưa đặt tên',
	idcategory int NOT NULL foreign key references Category(id),
	price float NOT NULL default 0
)
go
--Bill
create table Bill
(
	id int identity primary key,
	DateCheckIn date NOT NULL default getdate(),
	DateCheckOut date NOT NULL,
	idTable int NOT NULL foreign key references Tablecoffe(id),
	[status] int not null default 0,--thanh toán or chưa
	totalprice int
)
go
--BillInfo
create table BillInfo
(
	id int identity primary key,
	idBill int NOT NULL foreign key references Bill(id),
	idDrinks int NOT NULL foreign key references Drinks(id),
	CountItem int NOT NULL default 0
)
go
insert into Account values (N'admin',N'admin',N'admin',0)
insert into Account values (N'staff',N'staff',N'staff',1)
go
------------------Thêm category
go
insert dbo.Category values ('coffe')
insert dbo.Category values ('soda')
insert dbo.Category values ('ice blended')
insert dbo.Category values ('yaourt')
insert dbo.Category values ('smoothies')
insert dbo.Category values ('milk shake')
insert dbo.Category values ('virgin')
insert dbo.Category values ('fruit tea')
insert dbo.Category values ('smoothies fruit')
insert dbo.Category values ('milk tea')
insert dbo.Category values ('juice')
insert dbo.Category values ('flower tea')
insert dbo.Category values ('mix drinks')
insert dbo.Category values ('different')
------------------Thêm nước
go
insert drinks values (N'Black Coffee',1,14)
insert drinks values (N'Milk coffee',1,18)
insert drinks values (N'Coffe sữa lắc',1,22)
insert drinks values (N'Ca cao sữa',1,25)
insert drinks values (N'Bạc xỉu 3 tầng',1,23)
-------------------------------------------------
insert drinks values (N'Blue Curacao',2,33)
insert drinks values (N'Berry',2,33)
insert drinks values (N'Passion',2,33)
insert drinks values (N'Mint',2,33)
insert drinks values (N'Lavender',2,33)
insert drinks values (N'Blue+Passion',2,33)
insert drinks values (N'Rose+Rasberry',2,33)
insert drinks values (N'Grape+Cinnamon',2,33)
insert drinks values (N'Berry+Apple',2,33)
insert drinks values (N'Berry+Coconut',2,33)
-------------------------------------------------
insert drinks values (N'Chocolate Coffee',3,40)
insert drinks values (N'Caramel Coffee',3,40)
insert drinks values (N'Green Tea Coffee',3,40)
insert drinks values (N'Cookie Chocolate',3,40)
insert drinks values (N'Cookie Berry',3,40)
insert drinks values (N'Cookie Mint',3,40)
insert drinks values (N'Apple Ice',3,40)
insert drinks values (N'Tiramisu Ice',3,40)
insert drinks values (N'Matcha Ice',3,40)
-------------------------------------------------
insert drinks values (N'Chanh dây',4,30)
insert drinks values (N'Đào',4,30)
insert drinks values (N'Xoài',4,30)
insert drinks values (N'vãi',4,30)
insert drinks values (N'Dâu',4,30)
insert drinks values (N'Phúc bồn tử',4,30)
--------------------------------------------------
insert drinks values (N'Bơ',5,35)
insert drinks values (N'Dừa',5,35)
insert drinks values (N'Mãng cầu',5,35)
insert drinks values (N'Dâu',5,35)
insert drinks values (N'Việt quất',5,35)
insert drinks values (N'Chanh tuyết',5,35)
-------------------------------------------------
insert drinks values (N'Chanh dây',6,30)
insert drinks values (N'Đào',6,30)
insert drinks values (N'Xoài',6,30)
insert drinks values (N'vãi',6,30)
insert drinks values (N'Dâu',6,30)
insert drinks values (N'Phúc bồn tử',6,30)
--------------------------------------------------
insert drinks values (N'Dừa',7,35)
insert drinks values (N'Matcha',7,35)
insert drinks values (N'Bạc hà',7,35)
insert drinks values (N'Dâu',7,35)
--------------------------------------------------
insert drinks values (N'Bơ',8,35)
insert drinks values (N'Dừa',8,35)
insert drinks values (N'Mãng cầu',8,35)
insert drinks values (N'Dâu',8,35)
insert drinks values (N'Việt quất',8,35)
insert drinks values (N'Chanh tuyết',8,35)
------------------------------------------------
insert drinks values (N'Chanh dây',9,30)
insert drinks values (N'Đào',9,30)
insert drinks values (N'Xoài',9,30)
insert drinks values (N'vãi',9,30)
insert drinks values (N'Dâu',9,30)
insert drinks values (N'Phúc bồn tử',9,30)
-------------------------------------------------
insert drinks values (N'Matcha',10,25)
insert drinks values (N'Tiramisu',10,25)
insert drinks values (N'Đào',10,25)
insert drinks values (N'chanh dây',10,25)
insert drinks values (N'dưa lưới',10,25)
-------------------------------------------------
insert drinks values (N'Chanh dây',11,30)
insert drinks values (N'Đào',11,30)
insert drinks values (N'Xoài',11,30)
insert drinks values (N'vãi',11,30)
insert drinks values (N'Dâu',11,30)
insert  drinks values (N'Phúc bồn tử',11,30)
-------------------------------------------------
insert drinks values (N'Hoa hồng',12,30)
insert drinks values (N'Hoa cúc',12,30)
insert drinks values (N'Hoa Quế',12,30)
insert drinks values (N'Hoa bách hợp',12,30)
insert drinks values (N'Hoa đào',12,30)
insert drinks values (N'Hoa Cam',12,30)
--------------------------------------------------
insert drinks values (N'Chanh dây',13,30)
insert drinks values (N'Đào',13,30)
insert drinks values (N'Xoài',13,30)
insert drinks values (N'vãi',13,30)
insert drinks values (N'Dâu',13,30)
insert drinks values (N'Phúc bồn tử',13,30)
------------------------------------------------
insert drinks values (N'Thuốc mèo',14,2)
insert drinks values (N'Thuốc 3 số',14,2)
insert drinks values (N'Gói mèo',14,20)
insert drinks values (N'Gói 3 số',14,25)
insert drinks values (N'Khăn lạnh',14,2)
-------------------------------Thêm Bill
go
-------------------------------Thêm Bàn
declare @i int = 1
while @i <= 10
begin
	insert dbo.Tablecoffe (name) values (N'Bàn '+CAST(@i as nvarchar(100)))--cast để chuyển @i sang kiểu nvarchar
	set @i=@i+1
end
go