<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/design2.css?rnd=139" rel="stylesheet" />
    <script src="js/jquery-3.3.1.min.js"></script>
    <script src="js/popper.min.js"></script>
    <script src="js/bootstrap.min.js"></script>

    <style>
    
        .auto-style1 {
            width: 179px;
            text-align: right;
            font-weight: bold;
        }
    

        .auto-style4 {
            height: 38px;
        }
        .auto-style6 {
            width: 56%;
            background-color: white;
        }
        .cntrl-div {
            width: 60%;
            background-color: white;  
            border: solid 1px black;
        }

        .auto-style7 {
            width: 179px;
            text-align: right;
            font-weight: bold;
            height: 26px;
        }
        .auto-style8 {
            height: 26px;
        }


        .collapsible {
             background-color: black;
             color: white;
             cursor: pointer;
             padding: 5px;
             width: 100%;
             border: none;
             text-align: left;
             outline: none;
             font-size: 15px;
         }

         .active, .collapsible:hover {
                 background-color: black;
         }

         .content {
             padding: 0 18px;
             display: none;
             overflow: hidden;
             background-color: #f1f1f1;
         }

       
    </style>

      <script type="text/javascript">
         function collapseExpand() {
             document.getElementById("collapsible-button0").click();
         }        
     </script>

</head>
<body onload="collapseExpand();">
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-md navbar-dark fixed-top bg-dark">
<a class="navbar-brand" href="#">MyTecBits.com</a>
<button class="navbar-toggler collapsed" type="button" data-toggle="collapse" data-target="#navbar" aria-controls="navbar" aria-expanded="false" aria-label="Toggle navigation">
<span class="navbar-toggler-icon"></span>
</button>
<div id="navbar" class="navbar-collapse collapse">
<ul class="navbar-nav mr-auto">
<li class="nav-item active"><a class="nav-link" href="/Default.aspx">Home</a></li>
<li class="nav-item"><a class="nav-link" href="#about">About</a></li>
<li class="nav-item"><a class="nav-link" href="#contact">Contact</a></li>
<li class="nav-item dropdown">
<a class="nav-link dropdown-toggle" href="#" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" id="dropdown04">Dropdown <span class="caret"></span></a>
<div class="dropdown-menu" aria-labelledby="dropdown04">
<a class="dropdown-item" href="#">Action</a>
<a class="dropdown-item" href="#">Another action</a>
<a class="dropdown-item" href="#">Something else here</a>
</div>
</li>
</ul>
</div>
<!–/.nav-collapse –>
</nav>
        <div class="container">
            <br />
            <br />
            <br />
            <br />
            <button type="button" id="collapsible-button0" class="collapsible">Add&nbsp;User </button>
            <div class="content">
               <br />
             <div class="cntrl-div"><br />
              <table class="tbl-class">
                <tr>
                    <td class="auto-style1">UserName:&nbsp; </td>
                    <td class="auto-style6"><asp:TextBox ID="TextBox1" class="form-control" autocomplete="off" runat="server" Height="30px" Width="200px" MaxLength="40"></asp:TextBox>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1" Display="Dynamic" ErrorMessage="*Required" ForeColor="Red" ValidationGroup="reqField"></asp:RequiredFieldValidator>

                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">First Name:&nbsp; </td>
                    <td class="auto-style6"><asp:TextBox ID="TextBox2" class="form-control" autocomplete="off" runat="server" Height="30px" Width="200px"></asp:TextBox>

                    </td>
                    <td class="auto-style4"></td>
                </tr>
                <tr>
                    <td class="auto-style1">Last Name:&nbsp; </td>
                    <td class="auto-style6"><asp:TextBox ID="TextBox3" class="form-control" autocomplete="off" runat="server" Height="30px" Width="200px"></asp:TextBox>

                    </td>
                    <td class="auto-style4"></td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td class="bg-white" colspan="2"><asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Add User" CommandName="Insert" Width="200px" ValidationGroup="reqField" />&nbsp;&nbsp;<asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Add Another" CommandName="Insert" Visible="False" Width="188px" /></td>
                </tr>
                <tr>
                    <td class="auto-style7"></td>
                    <td colspan="2" class="auto-style8">
            <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>

                    </td>
                </tr>
            </table>
             </div> 
            <br />
                </div>
            
            
             <br />
        <br /><%-- source: https://parallelcodes.com/styling-the-gridview-in-asp-net-2/--%>
        <asp:GridView ID="GridView1" runat="server" CssClass="mydatagrid" PagerStyle-CssClass="pager"
            HeaderStyle-CssClass="header" RowStyle-CssClass="rows" AllowPaging="True" OnPageIndexChanging="Gridview1_PageIndexChanging" 
            OnSelectedIndexChanged="Gridview1_SelectedIndexChanged" OnRowCommand="GridView1_RowCommand" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="UserName" HeaderText="UserName" />
                <asp:BoundField DataField="FirstName" HeaderText="FirstName" />
                <asp:BoundField DataField="LastName" HeaderText="LastName" />
                <asp:BoundField DataField="Created" HeaderText="Created" />
                <asp:BoundField DataField="Created_by" HeaderText="CreatedBy" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="Button4" CommandName="Attch" runat="server" Text="Attach" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
<HeaderStyle CssClass="header"></HeaderStyle>

<PagerStyle CssClass="pager"></PagerStyle>

<RowStyle CssClass="rows"></RowStyle>
        </asp:GridView>
            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">Delete All</asp:LinkButton>
            <br />
            <br />
&nbsp;<asp:Label ID="Label2" runat="server"></asp:Label>
            <br />
        </div>

        
     


   <script>
       var coll = document.getElementsByClassName("collapsible");
var i;

for (i = 0; i < coll.length; i++) {
  coll[i].addEventListener("click", function() {
    this.classList.toggle("active");
    var content = this.nextElementSibling;
    if (content.style.display === "block") {
      content.style.display = "none";
    } else {
      content.style.display = "block";
    }
  });
} 
   </script> 

        <%--Bootstrap demo from https://www.mytecbits.com/microsoft/dot-net/how-to-add-bootstrap-in-asp-net#WebFormsManual--%>

    </form>
</body>
</html>
