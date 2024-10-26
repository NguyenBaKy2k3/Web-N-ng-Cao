CREATE DATABASE DATING
CREATE TABLE tblRole
(
	iRoleID int IDENTITY,
	sRoleName nvarchar(50),
	CONSTRAINT PK_tblRole PRIMARY KEY (iRoleID),
)
drop table tblRole
CREATE TABLE Ad_Min
(
	iAdmin int IDENTITY,
	sAdminName nvarchar(50) NOT NULL,
	email VARCHAR(100) NOT NULL UNIQUE,
	password VARCHAR(255) NOT NULL,
	iRoleID INT
	CONSTRAINT PK_Admin PRIMARY KEY (iAdmin)
	CONSTRAINT FK_tblRole FOREIGN KEY (iRoleID) REFERENCES tblRole(iRoleID)
)

INSERT INTO Ad_Min(sAdminName, email, password, iRoleID)
VALUES (N'Quản lý', 'vaicatrau@gmail.com', '17102003', 1);

Select * from  Ad_Min

INSERT INTO tblRole(sRoleName)
VALUES ('Admin'), ('Customer');
Select * from  tblRole

SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG = 'DATING';



DROP TABLE Users

SELECT*FROM Users
select*from User_Profile
select*from Likes
select*from Matches
select*from Skippe
select*from MessagesS
select*from Reports
select*from tblNotification


-- Tạo bảng Users
CREATE TABLE Users (  -- Bảng chứa thông tin người dùng
    user_id INT PRIMARY KEY IDENTITY(1,1),  -- Khóa chính, ID người dùng tự động tăng
    username NVARCHAR(50) NOT NULL,  -- Tên người dùng
    email VARCHAR(100) NOT NULL UNIQUE,  -- Địa chỉ email, không được trùng lặp
    password VARCHAR(255) NOT NULL,  -- Mật khẩu của người dùng
	sdt VARCHAR(12) NOT NULL,
    gender NVARCHAR(10) CHECK (gender IN ('male', 'female', 'other')) NOT NULL,  -- Giới tính
    date_of_birth DATE NOT NULL,  -- Ngày sinh
    bio NVARCHAR(100),  -- Thông tin tiểu sử
    profile_picture VARCHAR(255),  -- Đường dẫn ảnh đại diện
    location NVARCHAR(100),  -- Địa điểm
    created_at DATETIME DEFAULT GETDATE(),  -- Thời gian tạo tài khoản
	latitude FLOAT,  -- Vĩ độ
	longitude FLOAT,  -- Kinh độ
	iUsersRoleID INT,
	IsActive BIT
	--PasswordResetToken VARCHAR(100) NULL  -- Token đặt lại mật khẩu
	CONSTRAINT FK_Users_tblRole FOREIGN KEY (iUsersRoleID) REFERENCES tblRole(iRoleID)
);


DROP TABLE User_Profile
select*from User_Profile
-- Tạo bảng User_Profile
CREATE TABLE User_Profile (  -- Bảng chứa thông tin hồ sơ chi tiết của người dùng
    profile_id INT PRIMARY KEY IDENTITY(1,1),  -- Khóa chính, ID hồ sơ tự động tăng
    user_profile_id INT,  -- Khóa ngoại liên kết với bảng Users
    occupation NVARCHAR(100),  -- Nghề nghiệp của người dùng
    relationship_status NVARCHAR(30) CHECK (relationship_status IN (N'Độc thân', N'Đang trong mối quan hệ', N'Đã kết hôn', N'Phức tạp')),  -- Tình trạng mối quan hệ
    gender_looking NVARCHAR(10) CHECK (gender_looking IN (N'Nam', N'Nữ', N'Khác')) NOT NULL,
	looking_for NVARCHAR(100) NOT NULL,  -- Mục tiêu tìm kiếm của người dùng
    hobbies NVARCHAR(100),  -- Sở thích cá nhân
    height DECIMAL(5,2),  -- Chiều cao
    weight DECIMAL(5,2),  -- Cân nặng
	isApproved BIT
    FOREIGN KEY (user_profile_id) REFERENCES Users(user_id)  -- Ràng buộc khóa ngoại
);


DROP TABLE Matches

-- Tạo bảng Matches
CREATE TABLE Matches (  -- Bảng ghi nhận các cặp đôi đã ghép
    match_id INT PRIMARY KEY IDENTITY(1,1),  -- Khóa chính, ID cặp đôi tự động tăng
    user1_id INT,  -- Khóa ngoại cho người dùng 1
    user2_id INT,  -- Khóa ngoại cho người dùng 2
    match_date DATETIME DEFAULT GETDATE(),  -- Thời gian ghép đôi
    FOREIGN KEY (user1_id) REFERENCES Users(user_id),  -- Ràng buộc khóa ngoại
    FOREIGN KEY (user2_id) REFERENCES Users(user_id)   -- Ràng buộc khóa ngoại
);

DROP TABLE Likes
drop table Skippe
-- Tạo bảng Likes
CREATE TABLE Likes (  -- Bảng theo dõi người dùng thích nhau
    like_id INT PRIMARY KEY IDENTITY(1,1),  -- Khóa chính, ID sở thích tự động tăng
    userlike_id INT,  -- Khóa ngoại cho người dùng thích
    liked_user_id INT,  -- Khóa ngoại cho người dùng được thích
    created_at DATETIME DEFAULT GETDATE(),  -- Thời gian thích
    FOREIGN KEY (userlike_id) REFERENCES Users(user_id),  -- Ràng buộc khóa ngoại
    FOREIGN KEY (liked_user_id) REFERENCES Users(user_id)  -- Ràng buộc khóa ngoại
);



CREATE TABLE Skippe (  -- Bảng bỏ qua
    skippe_id INT PRIMARY KEY IDENTITY(1,1),  -- Khóa chính, ID sở thích tự động tăng
    user_skip_id INT,  -- Khóa ngoại cho người bỏ qua
    skippe_user_id INT,  -- Khóa ngoại cho người bị bỏ qua
    FOREIGN KEY (user_skip_id) REFERENCES Users(user_id),  -- Ràng buộc khóa ngoại
    FOREIGN KEY (skippe_user_id) REFERENCES Users(user_id)  -- Ràng buộc khóa ngoại
);



DROP TABLE MessagesS

-- Tạo bảng Messages
CREATE TABLE MessagesS (  -- Bảng lưu trữ các tin nhắn giữa người dùng
    message_id INT PRIMARY KEY IDENTITY(1,1),  -- Khóa chính, ID tin nhắn tự động tăng
    sender_id INT,  -- Khóa ngoại cho người gửi
    receiver_id INT,  -- Khóa ngoại cho người nhận
    content NVARCHAR(MAX) NOT NULL,  -- Nội dung tin nhắn
    sent_at DATETIME DEFAULT GETDATE(),  -- Thời gian gửi tin nhắn
    FOREIGN KEY (sender_id) REFERENCES Users(user_id),  -- Ràng buộc khóa ngoại
    FOREIGN KEY (receiver_id) REFERENCES Users(user_id)  -- Ràng buộc khóa ngoại
);


DROP TABLE Reports

-- Tạo bảng Reports
CREATE TABLE Reports (  -- Bảng ghi nhận các báo cáo vi phạm
    report_id INT PRIMARY KEY IDENTITY(1,1),  -- Khóa chính, ID báo cáo tự động tăng
    reporter_id INT,  -- Khóa ngoại cho người báo cáo
    reported_user_id INT,  -- Khóa ngoại cho người bị báo cáo
    reason NVARCHAR(MAX) NOT NULL,  -- Lý do báo cáo
    created_at DATETIME DEFAULT GETDATE(),  -- Thời gian tạo báo cáo
    FOREIGN KEY (reporter_id) REFERENCES Users(user_id),  -- Ràng buộc khóa ngoại
    FOREIGN KEY (reported_user_id) REFERENCES Users(user_id)  -- Ràng buộc khóa ngoại
);

DROP TABLE tblNotification


CREATE TABLE tblNotification (  
    notification_id INT PRIMARY KEY IDENTITY(1,1), 
    notification_receiver_id INT, 
    admin_id INT,  
    notification_content NVARCHAR(MAX) NOT NULL, 
    created_at DATETIME DEFAULT GETDATE(), 
    FOREIGN KEY (notification_receiver_id) REFERENCES Users(user_id), 
    FOREIGN KEY (admin_id) REFERENCES Ad_Min(iAdmin)  
);

DROP TABLE Feedback

CREATE TABLE Feedback (  
    feedback_id INT PRIMARY KEY IDENTITY(1,1), 
    user_feeback_id INT, 
    feedback_content NVARCHAR(MAX) NOT NULL
    FOREIGN KEY (user_feeback_id) REFERENCES Users(user_id)
);