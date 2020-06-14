# terminal
Client-Server application with HTTP-protocol

Application consist of 3 parts:

#1 TerminalServer - WebApi + EF Core

    Server provides Restfull web-service. In database there is one entity - Product.

#2 TerminalClient - WPF

    Client can add products and upload it into server.

#3 TerminalClientAdmin - WPF

    Admin client can download all products from server and delete any of it.


When client cannot connect to the server because of bad internet connection or server is off, all data will not lose.
It will be information alert and client will try another attempt later for downloading or uploading products.
