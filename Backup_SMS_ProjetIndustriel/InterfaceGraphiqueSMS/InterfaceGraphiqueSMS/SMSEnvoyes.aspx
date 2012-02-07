<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="SMSEnvoyes.aspx.cs" Inherits="InterfaceGraphiqueSMS.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- CSS -->
    <link href="css/SMSEnvoyes.css" rel="Stylesheet" type="text/css" />
    <!-- IScroll -->
    <script type="text/javascript" src="Scripts/iscroll.js"></script>
    <!-- JAVASCRIPT -->
    
    <script type="text/javascript">



        function selectTableSMS(idSMS) {

            //sauvegarde de l'id
            InterfaceGraphiqueSMS.WebForm1.saveIdSMS(idSMS);

            $("#<%= buttonCache.ClientID %>").click();
            $("#<%= UpdatePanel1.ClientID %>").modal({ minHeight: 450 });


        }


        function validSearchForm() {

            // recuperation sauvegarde de la recherche
            var search = document.getElementById("search").value;
            InterfaceGraphiqueSMS.WebForm1.saveSearch(search);

            $("#<%= buttonSearch.ClientID %>").click();
        }

        function FormKeyPressed() {

            //si touche entree
            if (window.event.keyCode == 13) {
                $("#submit").click();

            }
        }

        function supprimerSMS(idSMS) {

            if (confirm("Voulez-vous vraiment supprimer ce SMS ?")) {
                InterfaceGraphiqueSMS.WebForm1.saveIdSMS(idSMS);
                $("#<%= buttonSupprimer.ClientID %>").click();
            }
        }



    </script>
 
    <!-- FIN JAVASCRIPT -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">

<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>

    <h2 align="center">
        SMS envoyés</h2>


    <!-- RECHERCHE -->
   
  
<form id="searchbox">

<center>
   <input id="search" type="text" placeholder="Rechercher Emetteur, Message, Date Reception (jj/mm/aaaa)" onkeypress="FormKeyPressed()">
    <input id="submit" type="button" value="Search" onclick="validSearchForm()">
    
    </center>
</form>


<br />
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <!-- Tableau des SMS envoyes -->
    <div id="divTable">
    
    
 <asp:Panel ID="Panel1" runat="server" Height="320px" ScrollBars=Auto>

                    <asp:Table ID="TableSMSEnvoyes" runat="server" CssClass="tableauSMS">
                    </asp:Table>
                    </asp:Panel>

                            <div style="display:none">
    <asp:Button ID="buttonSearch" ClientIDMode="Predictable" runat="server" Text="Button"
                        OnClick="buttonSearch_clicked" />
                        <asp:Button ID="buttonSupprimer" ClientIDMode="Predictable" runat="server" Text="Button"
                        OnClick="buttonSupprimer_clicked" />
                        </div>

                                <asp:Timer ID="TimerRefresh" runat="server" OnTick="rafraichirPage">  
                                </asp:Timer>

                    </ContentTemplate>
                    </asp:UpdatePanel>
    </div>


    
    <!-- Fin tableau -->
    
    <div id="divPanel1" style="display: none">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <form>
                <table align="center" id="tabInfo">
                    <tr>
                        <td>
                            Emetteur :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEmetteur" runat="server" ReadOnly="True"></asp:TextBox><br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Destinataire :
                        </td>
                        <td>
                            <asp:TextBox ID="tbDestinataire" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Statut :
                        </td>
                        <td>
                            <asp:TextBox ID="tbStatut" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;Encodage :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEncodage" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Date demande :
                        </td>
                        <td>
                            <asp:TextBox ID="tbDateDemande" runat="server" ReadOnly="True"></asp:TextBox><br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;Date envoi :
                        </td>
                        <td>
                            <asp:TextBox ID="tbDateEnvoi" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <center>
                                Message :
                                <br />
                                <asp:TextBox ID="tbMessage" runat="server" CssClass="style2" Height="89px" ReadOnly="True"
                                    Width="330px" TextMode="MultiLine"></asp:TextBox>
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <center>
                                PDU :
                                <br />
                                <asp:TextBox ID="tbPDU" runat="server" CssClass="style2" Height="89px" ReadOnly="True"
                                    Width="330px" TextMode="MultiLine"></asp:TextBox>
                            </center>
                        </td>
                    </tr>
                </table>
                </form>
                <div style="display: none">
                    <asp:Button ID="buttonCache" ClientIDMode="Predictable" runat="server" Text="Button"
                        OnClick="buttonCache_clicked" />

                        
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>



 
    
        
    </div>
    <br />




</asp:Content>
