# KMinds Website

Welcome to the **KMinds Website**! This is the official website for **KMinds**, Khulna University of Engineering & Technology's premier Data Science, Artificial Intelligence, and Machine Learning club. 

The website serves as a central hub for club members to share knowledge, publish research, distribute datasets, and manage events.

## 🚀 Features

- **User Authentication & Roles:** Registration, login, and secure role-based access (Member, CoreMember, Admin, President, etc.).
- **Member Approval System:** New members verify their registration via a Transaction ID system, which admins can approve from a dashboard.
- **Dynamic Content Feeds:**
  - **Articles:** Publish and read articles on data science topics.
  - **Research:** Share and discuss recent academic papers and ML research.
  - **Datasets:** Upload, share, and download datasets for model training and analysis.
  - **Events:** Keep track of upcoming datathons, workshops, and weekly meetups.
- **Content Management (CRUD):** Authors can easily edit or delete their own posts. Admins can manage global events and user roles.
- **Universal Details Modal:** Clean, glassmorphism-styled modal interface for viewing full details of feed items without leaving the page.
- **Dynamic Homepage:** Live-updating statistics (member count, datasets hosted, etc.) and a dynamic highlight reel of the most recent activities.

## 💻 Tech Stack

- **Backend:** C# / ASP.NET Web Forms (.NET Framework 4.8)
- **Database:** Microsoft SQL Server / LocalDB
- **Frontend:** Vanilla HTML5, CSS3, and JavaScript (ES6 Module system)
- **Authentication:** ASP.NET Forms Authentication

## 🛠️ Local Development Setup

To run this project locally on your machine, follow these steps:

### 1. Prerequisites
- **Visual Studio** (Community/Professional/Enterprise) with the **ASP.NET and web development** workload installed.
- **SQL Server Express** or **LocalDB** (which typically comes with Visual Studio).

### 2. Clone the Repository
```bash
git clone https://github.com/AbirHasanArko/KMinds-Website.git
cd KMinds-Website
```

### 3. Database Setup
The project relies on a SQL Server database named `KMindsPortal`. You can set this up using the provided SQL scripts:

1. Open your SQL Server Management Studio (SSMS) or the SQL Server Object Explorer in Visual Studio.
2. Run the queries in `DatabaseSchema.sql` to create the foundational tables.
3. Run the queries in `UpdateSchema.sql` to apply any recent column additions (like image thumbnails).
4. *(Optional)* Run `seed_data.sql` to populate the database with dummy data for testing.

### 4. Configure the Connection String
Open the `Web.config` file in the root directory. Ensure that the `connectionString` matches your local SQL Server instance:

```xml
<add name="KMindsDB" connectionString="Server=(localdb)\MSSQLLocalDB;Database=KMindsPortal;Trusted_Connection=True;" providerName="System.Data.SqlClient" />
```

### 5. Run the Application
- Open the folder as a Website in Visual Studio (`File > Open > Web Site...`).
- Press `F5` or click **Start Debugging** to launch the site in IIS Express.

## 📂 Project Structure

- `/assets`: Contains all static frontend assets including CSS stylesheets, JavaScript modules, and images.
- `/Uploads`: Local storage directory for user-uploaded dataset files, research papers, and cover images (auto-generated when users upload content).
- `*.aspx`: The main ASP.NET Web Forms frontend pages.
- `*.aspx.cs`: The C# code-behind files handling the backend logic for each respective page.
- `Site.Master`: The global master page containing the site navigation, footer, and universal modal structure.

## 👨‍💻 Developer
Created with ❤️ by **Abir Hasan Arko**
