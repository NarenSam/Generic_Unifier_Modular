Imports System.String
Imports System.Xml
Imports System.Data.OleDb

Public Class viewXml
    Dim saved As Integer = 0
    Dim envname As String = Form3.Label2.Text
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        input.Show()
        Me.Hide()
    End Sub
    Private Sub viewXml_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TextBox2.Text = input.TextBox2.Text
        PictureBox1.ImageLocation = System.Environment.CurrentDirectory & "\Environment_Image\" & TextBox2.Text & ".png"
        Me.PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage
        PictureBox1.Load()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim sqlconn As New OleDb.OleDbConnection
        Dim connString As String
        Dim sqlquery As New OleDb.OleDbCommand

        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.Environment.CurrentDirectory & "\Device.mdb"
        sqlconn.ConnectionString = connString
        sqlquery.Connection = sqlconn
        sqlconn.Open()
        sqlquery.CommandText = "INSERT INTO XML_File(BaseName,EnvironmentName,XMLName,TotalScenario)VALUES('" & input.TextBox3.Text & "','" & input.TextBox2.Text & "','" & input.xmlfilename & "','" & ScenarioInput.scenario_no - 1 & "');"
        sqlquery.ExecuteNonQuery()
        input.ScenarioNumber += 1
        sqlquery.CommandText = "Update ScenarioNumber set ScenarioNo = '" & input.ScenarioNumber & "' "
        sqlquery.ExecuteNonQuery()
        MsgBox("XmlFile name is saved successfully!!")
        saved = 1
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        RichTextBox1.Text = ""
        Me.RichTextBox1.LoadFile(System.Environment.CurrentDirectory + "\XML_Files\" + input.xmlfilename, RichTextBoxStreamType.PlainText)
        TextBox1.Text = input.xmlfilename
        MessageBox.Show("Please Note the Name of the XML File Generated Below")
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If saved = 1 Then
            Me.Close()
        Else
            MessageBox.Show("Save The Xml File")
        End If

    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        RichTextBox1.Text = " "
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click

        Process.Start(System.Environment.CurrentDirectory & "\" & TextBox1.Text)
    End Sub
End Class