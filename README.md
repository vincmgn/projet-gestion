# projet-gestion

## 🌟 Overview

**projet-gestion** is a desktop application for managing the inventory of a wholesaler. It allows managing products, categories, customers, and orders. It also features a dashboard on the homepage to display sales and stock statistics.

## 🚀 Features

- **🔐 Authentication**: User login and logout.
- **📊 Dashboard**:
  - 🏆 Best-selling product
  - 💰 Total sales amount
  - 📈 Chart: Top 3 best-selling products
  - 🔔 Notification of products soon out of stock (< 5 units)
- **📦 Product management**: Add, modify, delete, and display products.
- **🗂️ Category management**: Add, modify, delete, and display categories.
- **👥 Customer management**: Add, modify, delete, and display customers.
- **🛒 Order management**: Add, modify, delete, and display orders.

## 🗄️ Technical Constraints

| Frontend | Backend               |
| -------- | --------------------- |
| C# (WPF) | C# (ASP.NET)          |
| XAML     | SQLite                |
|          | Entity Framework Core |

## 📚 Installation

1. Clone the repository:

```bash
git clone
```

2. Create a new migration for the database:

```bash
cd Back/
dotnet ef migrations add InitialCreate
```

3. Update the database:

```bash
dotnet ef database update
```

4. Run the application:
   ![alt text](image.png)
   Click on the "Démarrer" button in Visual Studio.
