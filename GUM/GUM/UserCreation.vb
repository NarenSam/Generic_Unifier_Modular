Imports System.Data.OleDb
Public Class UserCreation
    Public sqlconn As New OleDb.OleDbConnection
    Public connString As String
    Public sqlquery As New OleDb.OleDbCommand
    Public dr As OleDb.OleDbDataReader
    Dim flag As Integer = 0
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
       
        sqlquery.CommandText = "select * from Total_User"
        dr = sqlquery.ExecuteReader
        While dr.Read
            If dr("UserName") = TextBox1.Text Then
                flag = 1
            End If
        End While
        dr.Close()
        If flag = 1 Then
            MessageBox.Show("Already Exist")
        Else
            sqlquery.CommandText = "Insert into Total_User(UserName,Priority,Category)values('" & UCase(TextBox1.Text) & "','" & TextBox2.Text & "','" & ComboBox1.SelectedItem & "');"
            sqlquery.ExecuteNonQuery()
            MessageBox.Show("Saved Successfully")
            TextBox1.Text = ""
            TextBox2.Text = ""
        End If
        dr.Close()
        flag = 0
    End Sub
    Private Sub btnclose(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.FormClosing
        Me.Hide()
        Form3.Show()
    End Sub
    Private Sub UserCreation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.Environment.CurrentDirectory & "\Device.mdb"
        sqlconn.ConnectionString = connString
        sqlquery.Connection = sqlconn
        sqlconn.Open()
        ComboBox1.Items.Add("Normal User")
        ComboBox1.Items.Add("Doctor")
        ComboBox1.Items.Add("Nurse")
        ComboBox1.Items.Add("Police")

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Hide()
        Form3.Show()
    End Sub

End Class