
E-commerce Website

This is an E-commerce website that allows users to search for books in various categories. The website has two types of default users: customers and admin.

Features

Customer The customer can:

View all books in stock Add books to cart Fill out order details (shipping details, notes) Rate the website and give feedback to the admin Keep track of previous orders

Admin The admin has a customized dashboard which allows them to:

Keep track of website entities View average ratings and percentage of each rating View all orders and their details View list of books, authors, categories, and users Add new roles and users Keep track of the most profitable categories through a pie chart

Tech Stack This project was developed using:

ASP.NET Core MVC Entity Framework Bootstrap CSS3 HTML5

The Repo-service MVC pattern was implemented and SOLID principles were applied to achieve a code that is easy to maintain and easy to extend. The identity framework was utilized to build a secure website for different types of users. Various types of validations were made to prevent duplicate usernames, book names, category names, and author names.

The business logic of the cart and order was implemented through sessions to prevent filling the database with unimportant data that the user might delete later. Once the user confirms their order, all order details are saved.
