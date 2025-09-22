# Product Backlog Creator

## 📌 Overview

The **Product Backlog Creator** is an MVC web application that allows users to create and manage product backlogs for their projects.

This project was one of the first applications I developed and served as a **practical introduction to building CRUD applications** with the MVC pattern. The experience gained here played a key role in shaping my understanding of web development and helped me build more advanced projects, such as the **Doctor’s Practice Management System**.

---

## 🚀 Features

* ➕ **Add new backlog items**
* ✏️ **Edit existing backlog items**
* 🗑️ **Delete tasks**
* 📋 **View a list of project backlog items**
* 🎯 Simple and clean user interface

---

## 🛠️ Tech Stack

* **Framework:** .NET Framework (MVC)
* **Language:** C#
* **Database:** Entity Framework with SQL Server
* **IDE:** Visual Studio
* **UI:** Razor Views + Bootstrap

---

## 📂 Project Structure

```
project-root/
│── Controllers/       # Handles requests and logic
│── Models/            # Entity Framework models
│── Views/             # Razor Views (UI)
│── Migrations/        # Database schema updates
│── README.md          # Project overview
```

---

## 🔧 Installation & Setup

1. Clone the repository

   ```bash
   git clone https://github.com/yourusername/ProductBacklogCreator.git
   ```
2. Open the project in **Visual Studio**.
3. Update the **connection string** in `Web.config` to point to your SQL Server instance.
4. Run the application (`F5`) to build and launch.

---

## 🎯 Usage

* Create a new project backlog
* Add tasks as backlog items
* Edit or remove tasks as the project evolves
* Track tasks in a simple, lightweight interface

---

## 📖 Future Improvements

* Add user authentication for multiple users
* Include task prioritization and deadlines
* Export backlog items to CSV or Excel
* Add drag-and-drop task reordering

---

## 👩‍💻 Author

*Developed by \Cameron Peters* as an early learning project in **ASP.NET MVC with Entity Framework**.
