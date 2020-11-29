using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SQLite;
using System.Data;
using System.Configuration;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            GetUniqueId();
            BindGridView();            
        }
    }

    protected void GetUniqueId()
    {
        SQLiteConnection Con = new SQLiteConnection(ConfigurationManager.ConnectionStrings["dbSqlite"].ConnectionString);
        bool uidExist = false;
        string uniqueId = UniqueId();

        using (Con)
        {
            Con.Open();
            try
            {
                uidExist = ValueExist(Con, uniqueId);
            }
            catch
            {
                // log error.
            }

            if (uidExist == false)
            {
                ViewState["UniqueId"] = uniqueId;
                return;
            }
            else
            {
                int i = 0;
                do
                {
                    uniqueId =  UniqueId();
                    uidExist = ValueExist(Con, uniqueId);
                    if (uidExist == false)
                    {
                        ViewState["UniqueId"] = uniqueId;
                        return;
                    }

                    i++;

                } while (i < 5);

                // If you can't get a unique uid in 5 tries
                // give the user a cryptic error message
                Label1.Text = "System Error";
            }
        }
    }

    protected void BindGridView()
    {
        DataTable dt = new DataTable();

        SQLiteConnection Con = new SQLiteConnection(ConfigurationManager.ConnectionStrings["dbSqlite"].ConnectionString);

        string sql = "Select UserName, FirstName, LastName, strftime('%m-%d-%Y', Created) as Created, Created_by From User";
        SQLiteCommand cmd = new SQLiteCommand(sql, Con);
        SQLiteDataAdapter MySqlDa = new SQLiteDataAdapter(cmd);

        using (Con)
        {
            Con.Open();
            try
            {

                MySqlDa.Fill(dt);


            }
             catch (Exception ex)
            {
              // Log Error

            }
        }

        GridView1.DataSource = dt;
        GridView1.DataBind();

        if (dt.Rows.Count > 0)
        {
            LinkButton1.Visible = true;
        }
        else
        {
            LinkButton1.Visible = false;
        }

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Button btnSelect = (sender as Button);
        string commandName = btnSelect.CommandName;
        UserAddUpdate(commandName);

        BindGridView();
        Button3.Visible = true;

    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        ResetControls();
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        DeleteUsers();
        BindGridView();
    }

    #region Data Access

    protected void UserAddUpdate(string commandName)
    {
        SQLiteConnection Con = new SQLiteConnection(ConfigurationManager.ConnectionStrings["dbSqlite"].ConnectionString);
        string userName = TextBox1.Text.Trim().ToLower();
        string uniqueId = (string)ViewState["UniqueId"];

        if (commandName == "Insert")
        {
            using (Con)
            {
                Con.Open();

                // Check if userName exist, exit if it does.
                bool userExist = UserExist(Con, userName);
                if (userExist == true)
                {
                    Label1.Text = "Duplicate User Name.";
                    Button3.Visible = false;
                    TextBox1.Enabled = false;
                    return;
                }
                else
                {
                    Label1.Text = string.Empty;
                }

                bool valueExist = false;
                valueExist = ValueExist(Con, uniqueId);

                if (valueExist == false)
                {
                    InsertNewUser(Con, uniqueId);
                    TextBox1.Enabled = false;
                }
                else
                {
                    Label1.Text = "Insert Error";
                    return;
                }
               
                UpdateNewUser(Con, uniqueId);
            }

            Button1.CommandName = "Update";
            Button1.Text = "Update User";

        }
        else if (commandName == "Update")
        {
            using (Con)
            {
                Con.Open();
                UpdateNewUser(Con, uniqueId);
            }
        }
    }

    protected void InsertNewUser(SQLiteConnection Con, string uniqueId)
    {
        string created = DateTime.Now.ToString("yyyy-MM-dd");
        string created_by = "User.Identity"; // User.Identity.Name, After you set up some type of authentication.


        string sql = @"Insert Into `User` (Uid, UserName, Created, Created_by ) values(@Uid, @UserName, @Created, @Created_by)";
        string userName = TextBox1.Text.Trim().ToLower();

        using (SQLiteCommand cmd = new SQLiteCommand(sql, Con))
        {

           try
            {
                cmd.Parameters.AddWithValue("@uid", uniqueId);
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Created", created);
                cmd.Parameters.AddWithValue("@Created_by", created_by);
                cmd.ExecuteNonQuery();
            }
           catch
            {
            // log error;
            }
        }
    }

    protected void UpdateNewUser(SQLiteConnection Con, string uniqueId)
    {
        string userName = TextBox1.Text.Trim().ToLower();
        string firstName = TextBox2.Text.Trim();
        string lastName = TextBox3.Text.Trim();
       


        string sql = @"Update `User` set UserName = @UserName, FirstName = @FirstName, LastName = @LastName Where uid = @uid ";

        using (SQLiteCommand cmd = new SQLiteCommand(sql, Con))
        {

            try
            {
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@uid", uniqueId);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                // log error;
            }
        }

    }

  
    protected void DeleteUsers()
    {
        SQLiteConnection Con = new SQLiteConnection(ConfigurationManager.ConnectionStrings["dbSqlite"].ConnectionString);

        string sql = @"Delete From `User`";

        using (SQLiteCommand cmd = new SQLiteCommand(sql, Con))
        {
            Con.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
                // log error;
            }
        }
    }


    protected bool ValueExist(SQLiteConnection Con, string uniqueId)
    {
        object valueExist;
        string sql = @"SELECT `uid` FROM `User` where uid = @uid";

        using (SQLiteCommand cmd = new SQLiteCommand(sql, Con))
        {

            try
            {
                cmd.Parameters.AddWithValue("@uid", uniqueId);
                valueExist = cmd.ExecuteScalar();
                {
                    if (valueExist != null)
                        return true;
                }
            }
            catch
            {
                return false;
            }
        }

        return false;
    }

    protected bool UserExist(SQLiteConnection Con, string userName)
    {
        object userExist;
        string sql = @"SELECT `UserName` FROM `User` where UserName = @UserName";

        using (SQLiteCommand cmd = new SQLiteCommand(sql, Con))
        {

            try
            {
                cmd.Parameters.AddWithValue("@UserName", userName);
                userExist = cmd.ExecuteScalar();
                {
                    if (userExist != null)
                        return true;
                }
            }
            catch
            {
                return false;
            }
        }

        return false;
    }

    protected string UniqueId()
    {
        Random r = new Random();
        string UniqueId = r.Next(10000000, 99999999).ToString();
        return UniqueId;
    }

    protected void ResetControls()
    {
        GetUniqueId();
        Button1.CommandName = "Insert";
        Button1.Text = "Add User";
        TextBox1.Text = string.Empty;
        TextBox1.Enabled = true;
        TextBox2.Text = string.Empty;
        TextBox3.Text = string.Empty;
        Button3.Visible = false;
        Label1.Text = string.Empty;

    }

    #endregion

    #region Gridview events

    protected void Gridview1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }


    protected void Gridview1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            BindGridView();
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
        }
        catch (Exception ex)
        {

        }
    }


    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Attch")
        {
            //first find the button that got clicked
            var clickedButton = e.CommandSource as Button;
            //find the row of the button
            var clickedRow = clickedButton.NamingContainer as GridViewRow;
            //now as the UserName is in the BoundField, access it using the cell index.
            var userName = clickedRow.Cells[0].Text;

            string hashId = Guid.NewGuid().ToString();
            string[] temp = hashId.Split('-');
            hashId = temp[0] + "_" + temp[1];

            Response.Redirect("upload.aspx?hid=" + hashId + "&uid=" + userName);
        }
    }


    #endregion



}



