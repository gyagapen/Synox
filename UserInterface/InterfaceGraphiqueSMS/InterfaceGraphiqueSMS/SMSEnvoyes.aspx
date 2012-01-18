<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="SMSEnvoyes.aspx.cs" Inherits="InterfaceGraphiqueSMS.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">


        function populateSMSField_callback(res) {
            alert(res.value);
        }


        function selectTableSMS(idSMS) {


            //WebForm1.populateSMSField(populateSMSField_callback);

            InterfaceGraphiqueSMS.WebForm1.populateSMSField(idSMS);

            //alert($("ct100_Main_buttonCache").text());
            document.getElementById("<%= buttonCache.ClientID %>").innerHTML = idSMS;
            document.getElementById("<%= buttonCache.ClientID %>").click();
            //$("<%= buttonCache.ClientID %>").click();
            alert(idSMS);
        }



      
    </script>
    <style type="text/css">
        .style1
        {
        }
        .style2
        {
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
    <h1>
        SMS envoyés</h1>
    <asp:ListBox ID="ListMessages" runat="server" CssClass="style1" Width="404px" AutoPostBack="True"
        OnSelectedIndexChanged="ListMessages_SelectedIndexChanged" Height="129px"></asp:ListBox>
    <asp:Table ID="TableSMSEnvoyes" runat="server">
    </asp:Table>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            Emetteur :
            <asp:TextBox ID="tbEmetteur" runat="server" ReadOnly="True"></asp:TextBox>
            Destinataire :
            <asp:TextBox ID="tbDestinataire" runat="server" ReadOnly="True"></asp:TextBox>
            <br />
            Statut :
            <asp:TextBox ID="tbStatut" runat="server" ReadOnly="True"></asp:TextBox>
            &nbsp;Encodage :
            <asp:TextBox ID="tbEncodage" runat="server" ReadOnly="True"></asp:TextBox>
            <br />
            Date demande :
            <asp:TextBox ID="tbDateDemande" runat="server"></asp:TextBox>
            &nbsp;Date envoi :
            <asp:TextBox ID="tbDateEnvoi" runat="server"></asp:TextBox>
            <br />
            Message :
            <asp:TextBox ID="tbMessage" runat="server" CssClass="style2" Height="89px" ReadOnly="True"
                Width="330px" TextMode="MultiLine"></asp:TextBox>
            <br />
            PDU :
            <asp:TextBox ID="tbPDU" runat="server" CssClass="style2" Height="89px" ReadOnly="True"
                Width="330px" TextMode="MultiLine"></asp:TextBox>


        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ListMessages" EventName="SelectedIndexChanged" />
        </Triggers>

    </asp:UpdatePanel>
                            <div style="display: block">
            <asp:Button   ID="buttonCache" ClientIDMode="Predictable" ClientID="test" runat="server"
                Text="Button" OnClick="buttonCache_clicked" />
        </div>
    <br />
</asp:Content>
