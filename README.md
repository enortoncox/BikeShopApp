# BikeShopApp
Bike Shop Application | Angular frontend with a .NET WebAPI backend

The application is a bike shop website called Mega Cycles. It allows users to browse different bikes, purchase them and check their orders.
Users can also make reviews for each bike which affects the average rating for that bike.

## Features:

- Authentication and Authorization using Identity and JSON Web Tokens.
- Roles based Authorization for User and Admin.
- If a user has the admin role, then they have access to the Admin Panel page. Here they can:
  - Create a new product, update or delete existing ones.
  - Update a user's information, delete a user, or promote / demote that user's admin status.
- A Product List page with pagination and basic product filtering by price / rating.
- A Product Details page that displays information about the product.
- A user can create reviews for each product, and they are displayed on the Product Details page for all users.
  - These reviews can be updated and deleted by the owner of the review or an admin.
- A Cart page that lists all the items in the user's shopping cart.
- A Checkout page that lets the user finalise the purchase of the items in their shopping cart.
- An Orders page that shows the details of all purchases from that user.
- An account details page where users can update their password and account information.
- A user's Public Account page that displays all the reviews they have made across all products.
- Error handling in the form of a modal message and an error page.
- Images can be uploaded to the database for products and users.

## Images

![BikeShop_01](https://github.com/enortoncox/BikeShopApp/assets/67313141/e293a549-e207-496d-8b38-b087698bee5d)
![BikeShop_02](https://github.com/enortoncox/BikeShopApp/assets/67313141/d59bf924-8142-4aff-a877-455daccbb62f)
![BikeShop_03](https://github.com/enortoncox/BikeShopApp/assets/67313141/c40cad77-1037-4365-a86a-b2e740b7a79b)
![BikeShop_04](https://github.com/enortoncox/BikeShopApp/assets/67313141/2577c71a-396c-4956-8e2c-2c210af15709)
![BikeShop_05](https://github.com/enortoncox/BikeShopApp/assets/67313141/1c34be8c-3519-40b1-a18d-e0464a58577c)
![BikeShop_06](https://github.com/enortoncox/BikeShopApp/assets/67313141/6f36e5ef-2833-4322-ae1e-c6146c359ad8)
![BikeShop_07](https://github.com/enortoncox/BikeShopApp/assets/67313141/82830ef5-0f28-425b-aed3-86ac47acf55f)
