Imports System.String
Imports System.Xml
Imports System.Data.OleDb
Public Class input
    Public sqlconn As New OleDb.OleDbConnection
    Public connString As String
    Public sqlquery As New OleDb.OleDbCommand
    Public dr As OleDb.OleDbDataReader
    Public setLoc As Integer = 0
    Public UserCount As Integer = 0
    Public UserArray() As String
    Public ScenarioNumber As String
    Public xmlfilename As String
    Dim device() As String
    Dim str As String
    Dim n As Integer = 1
    Dim checkdevice As String

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Form3.Show()
        Me.Hide()
    End Sub
    Private Sub input_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.WindowState = FormWindowState.Maximized
        Dim flag As Integer = 0
        Button1.Enabled = False
        Dim OpenFileDlg As New OpenFileDialog
        TextBox2.Text = Form3.Label2.Text
        Dim ename As String = TextBox2.Text
        Dim e1 As Integer = 0
        Dim e2 As Integer = 0
        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.Environment.CurrentDirectory & "\Device.mdb"
        sqlconn.ConnectionString = connString
        sqlquery.Connection = sqlconn
        sqlconn.Open()
        sqlquery.CommandText = "select * from EnvironmentDevice where BaseName ='" & Base.Label2.Text & "' and Environment ='" & Form3.Label2.Text & "'"
        dr = sqlquery.ExecuteReader

        RichTextBox3.AppendText(ename)
        While dr.Read
            If dr("DeviceName") = "user" Then
                If e1 = 1 Then
                    RichTextBox1.Text += vbCrLf
                End If
                RichTextBox1.AppendText(dr("Sname"))
                e1 = 1
                UserCount += 1
            ElseIf dr("DeviceName") = "Room" Then
                RichTextBox3.Text += vbCrLf
                RichTextBox3.AppendText(dr("Sname"))

            ElseIf dr("DeviceName") <> "user" And dr("DeviceName") <> "Room" Then
                If e2 = 1 Then
                    RichTextBox2.Text += vbCrLf
                End If
                RichTextBox2.AppendText(dr("Sname"))
                e2 = 1
            End If
            TextBox3.Text = dr("BaseName")
        End While
        dr.Close()
        PictureBox1.ImageLocation = System.Environment.CurrentDirectory & "\Environment_Image\" & TextBox2.Text & ".png"

        Me.PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage

        PictureBox1.Load()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ReDim Preserve UserArray(UserCount)
        Dim flag As Integer = 0
        sqlquery.CommandText = "Select * from ScenarioNumber"
        dr = sqlquery.ExecuteReader
        dr.Read()
        ScenarioNumber = dr("ScenarioNo")
        xmlfilename = TextBox3.Text + "_" + TextBox2.Text + "_" & ScenarioNumber & ".xml"

        Dim writer As New XmlTextWriter(System.Environment.CurrentDirectory + "\XML_Files\" + xmlfilename, System.Text.Encoding.UTF8)

        writer.WriteStartDocument(True)
        writer.Formatting = Formatting.Indented
        writer.Indentation = 2
        writer.WriteStartElement("Scenario")
        writer.WriteStartElement("Base")
        writer.WriteString(TextBox3.Text)
        writer.WriteEndElement()
        writer.WriteStartElement("Environment")
        writer.WriteString(TextBox2.Text)
        writer.WriteEndElement()
        dr.Close()
        sqlquery.CommandText = "select * from UserAction"
        dr = sqlquery.ExecuteReader
        While dr.Read
            ScenarioCreation(writer)
            n = 1
        End While
        writer.Close()
        dr.Close()

        viewXml.Show()
    End Sub
    Private Sub ScenarioCreation(ByVal writer As XmlTextWriter)

        writer.WriteStartElement(dr("ScenarioNo"))
    
        writer.WriteStartElement("User")
        writer.WriteString(dr("UserName"))
        writer.WriteEndElement()

        writer.WriteStartElement("Action")
        writer.WriteString(dr("UserAction"))
        writer.WriteEndElement()

        writer.WriteStartElement("Location")
        writer.WriteString(dr("Location"))
        writer.WriteEndElement()

        writer.WriteStartElement("Time")
        writer.WriteString(dr("Timing"))
        writer.WriteEndElement()

        If dr("TurnON_Devices") <> "" Then
            writer.WriteStartElement("TurnON_Devices")
            writer.WriteString(dr("TurnON_Devices"))
            writer.WriteEndElement()
            MessageBox.Show(dr("TurnON_Devices").ToString)
        End If

        If dr("TurnOFF_Devices") <> "" Then
            writer.WriteStartElement("TurnOFF_Devices")
            writer.WriteString(dr("TurnOFF_Devices"))
            writer.WriteEndElement()
            MessageBox.Show(dr("TurnOFF_Devices").ToString)
        End If
        writer.WriteEndElement()

    End Sub
    Private Sub ScenarioButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScenarioButton.Click
        ScenarioInput.Show()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Close()
    End Sub
End Class