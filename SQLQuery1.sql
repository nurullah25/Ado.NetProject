CREATE DATABASE AspProject
GO

USE AspProject
GO

CREATE TABLE categories
(
	categoryId INT PRIMARY KEY IDENTITY(1, 1),
	categoryName VARCHAR(50) NOT NULL,
)
GO

CREATE TABLE product
(
	productId INT PRIMARY KEY IDENTITY(1, 1),
	productName VARCHAR(50) NOT NULL,	
	unitPrice MONEY NOT NULL,
	quantity INT NOT NULL,
	picture VARCHAR(200) NOT NULL,
	catId INT REFERENCES categories(categoryId) NOT NULL,
	stock VARCHAR(30) NOT NULL,
	salesDate DATE NOT NULL,
	brandId INT REFERENCES brand(brandId) NOT NULL
)
GO

CREATE TABLE customer
(
	customerId INT PRIMARY KEY IDENTITY,
	customerName VARCHAR(50) NOT NULL,
	address VARCHAR(50) NOT NULL,
	phone INT NOT NULL,
	email VARCHAR(40) NULL
)
GO
select * from orderDetails
GO

CREATE TABLE orderDetails
(
	orderId INT PRIMARY KEY IDENTITY,
	orderDate DATETIME NOT NULL,
	quantity SMALLINT NOT NULL,
	customerId INT REFERENCES customer(customerId),

)
GO

CREATE TABLE brand
(
	brandId INT IDENTITY PRIMARY KEY NOT NULL,
	brandName NVARCHAR(50) NOT NULL,
)
GO