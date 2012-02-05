<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Base.Master" CodeBehind="LireSMS.aspx.cs"
    Inherits="InterfaceGraphiqueSMS.LireSMS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- CSS -->
    <link href="css/SMSEnvoyes.css" rel="Stylesheet" type="text/css" />
    <!-- IScroll -->
    <script type="text/javascript" src="Scripts/iscroll.js"></script>
    <!-- JAVASCRIPT -->
    <script type="text/javascript">

        function selectTableSMS(idSMS) {

            //ouverture de la popup
            InterfaceGraphiqueSMS.WebForm1.saveIdSMS(idSMS);

            $("#<%= buttonCache.ClientID %>").click();
            $("#<%= UpdatePanel1.ClientID %>").modal({ minHeight: 300 });


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

        //l'utilisateur veut repondre a l'emetteur
        //noTel = numero de l'emetteur
        function repondreSMS(noTel) {

            //on renvoie sur le formulaire d'ecriture
            location.href = "EcrireSMS.aspx?notel=" + noTel;
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
        SMS Recus</h2>
    <!-- RECHERCHE -->
    <form id="searchbox" action="#">
    <center>
        <input id="search" type="text" placeholder="Rechercher Emetteur, Message, Date Reception (jj/mm/aaaa)" onkeypress="FormKeyPressed()">
        <input id="submit" type="button" value="Search" onclick="validSearchForm()">
    </center>
    </form>
    <br />
    <!-- Tableau des SMS envoyes -->
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="divTable">
                <asp:Panel ID="Panel1" runat="server" Height="320px" ScrollBars="Auto">
                    <asp:Table ID="TableSMSEnvoyes" runat="server" CssClass="tableauSMS">
                    </asp:Table>
                </asp:Panel>
                <div style="display: none">
                    <asp:Button ID="buttonSearch" ClientIDMode="Predictable" runat="server" Text="Button"
                        OnClick="buttonSearch_clicked" />
                        <asp:Button ID="buttonSupprimer" ClientIDMode="Predictable" runat="server" Text="Button"
                        OnClick="buttonSupprimer_clicked" />
                </div>
                <asp:Timer ID="TimerRefresh" runat="server" OnTick="rafraichirPage">
                </asp:Timer>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
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
                    
                    <td>
                        Date reception :
                    </td>
                    <td>
                        <asp:TextBox ID="tbDateReception" runat="server" ReadOnly="True"></asp:TextBox><br />
                    </td>
                    </tr>
                    <tr>
                        <td>
                            Date Lecture :
                        </td>
                        <td>
                            <asp:TextBox ID="tbDateLecture" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Accuse demand&eacute; :
                        </td>
                        <td>
                            <asp:TextBox ID="tbAccuse" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
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
