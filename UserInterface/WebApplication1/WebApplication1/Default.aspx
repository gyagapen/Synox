<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html>
<head>
    <title>Demonstration fonctionnement ASP</title>
</head>
<body>
    <!-- EXEMPLE SCRIPT ASP -->
    <%
        //Boucle qui va de 0 a 9 
        for (int i = 0; i < 10; i++)
        {
            //Affichage du numero actuel
            Response.Write("Numero actuel :" + i + "<br>");
        }
    %>
    <!-- FIN EXEMPLE -->
</body>
</html>

