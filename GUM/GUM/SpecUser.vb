Imports System.Data.OleDb
Imports System.IO
Imports System.Xml

Imports System.Data.SqlClient
Public Class SpecUser
    Public sqlconn As New OleDb.OleDbConnection
    Public connString As String
    Public sqlquery As New OleDb.OleDbCommand
    Public dr As OleDb.OleDbDataReader
    Dim count As Integer = 0
    Dim total As Integer = 0
    Dim filename As String
    Dim reach As Integer = 0
    Dim cnt As Integer = 0
    Public TotalScenario As Integer = 1
    Public flag As Integer = 0
    Public CheckDevice() As String
    Public DeviceList() As String = {""}
    Public DeviceList1() As String = {""}
    Dim TurnON_Device As String
    Dim TurnOFF_Device As String
    Dim BaseName As String
    Dim Envname As String
    Public arrlen As Integer = 0
    Public arrlen1 As Integer = 0
    Private Sub btnclose(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.FormClosing
        ChooseChart.Show()
    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
        ChooseChart.Show()
    End Sub
    Private Sub SpecUser_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Dim yourpath As String = "D:\xml"
        'If (Not System.IO.Directory.Exists(yourpath)) Then
        '    System.IO.Directory.CreateDirectory(yourpath)
        'End If
        
        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.Environment.CurrentDirectory & "\Device.mdb"
        sqlconn.ConnectionString = connString
        sqlquery.Connection = sqlconn
        sqlconn.Open()
        
        Dim Locate As String = Application.StartupPath & "\XML_Files\"
        Dim DirInfo As DirectoryInfo = New DirectoryInfo(Locate)
        Dim Files() As FileInfo = DirInfo.GetFiles("*.xml", SearchOption.AllDirectories)
        Dim File As FileInfo

        sqlquery.CommandText = "select * from Total_User"
        sqlquery.ExecuteNonQuery()
        dr = sqlquery.ExecuteReader
        While dr.Read
            ComboBox1.Items.Add(UCase(dr("UserName")))
        End While
        dr.Close()
        sqlquery.CommandText = "Delete * from UserAction"
        sqlquery.ExecuteNonQuery()
        For Each File In Files
            filename = File.Name
            sqlquery.CommandText = "select * from XML_File where XMLName ='" & filename & "'"
            sqlquery.ExecuteNonQuery()
            dr = sqlquery.ExecuteReader
            dr.Read()

            total = dr("TotalScenario")
            dr.Close()

            sqlquery.CommandText = "INSERT INTO UserAction(ScenarioNo,BaseName,Environment,UserName,UserAction,Location,Timing,TurnON_Devices,TurnOFF_Devices,XMLName)VALUES('','','','','','','','','','" & filename & "');"
            sqlquery.ExecuteNonQuery()

            Dim xml As New Xml.XmlTextReader(System.Environment.CurrentDirectory + "\XML_Files\" + filename)
            While xml.Read

                If xml.NodeType = XmlNodeType.Element Then

                    If xml.Name = "Base" Then

                        BaseName = xml.ReadInnerXml
                        sqlquery.CommandText = "Update UserAction set BaseName = '" & BaseName & "' where XMLName = '" & filename & "' "
                        sqlquery.ExecuteNonQuery()
                    End If
                    If xml.Name = "Environment" Then
                        Envname = xml.ReadInnerXml
                        sqlquery.CommandText = "Update UserAction set Environment = '" & Envname & "'where XMLName = '" & filename & "' "
                        sqlquery.ExecuteNonQuery()

                    End If


                    If xml.Name = "Scenario_" + TotalScenario.ToString And TotalScenario <= 1 Then
                        sqlquery.CommandText = "Update UserAction set ScenarioNo = '" & xml.Name & "' where XMLName = '" & filename & "'"
                        sqlquery.ExecuteNonQuery()
                    End If


                    If xml.Name = "User" Then
                        sqlquery.CommandText = "Update UserAction set UserName = '" & xml.ReadInnerXml.ToString & "' where ScenarioNo = 'Scenario_" & TotalScenario.ToString & "' And XMLName = '" & filename & "' "
                        sqlquery.ExecuteNonQuery()
                    End If
                    If xml.Name = "Action" Then
                        sqlquery.CommandText = "Update UserAction set UserAction = '" & xml.ReadInnerXml.ToString & "' where ScenarioNo = 'Scenario_" & TotalScenario.ToString & "' And XMLName = '" & filename & "' "
                        sqlquery.ExecuteNonQuery()
                    End If
                    If xml.Name = "Location" Then
                        sqlquery.CommandText = "Update UserAction set Location = '" & xml.ReadInnerXml.ToString & "' where ScenarioNo = 'Scenario_" & TotalScenario.ToString & "' And XMLName = '" & filename & "' "
                        sqlquery.ExecuteNonQuery()
                    End If

                    If xml.Name = "Time" Then
                        sqlquery.CommandText = "Update UserAction set Timing = '" & xml.ReadInnerXml.ToString & "' where ScenarioNo = 'Scenario_" & TotalScenario.ToString & "' And XMLName = '" & filename & "'"
                        sqlquery.ExecuteNonQuery()
                    End If

                    If xml.Name = "TurnON_Devices" Then
                        TurnON_Device = xml.ReadInnerXml
                        reach = 1
                    End If
                    If xml.Name = "TurnOFF_Devices" Then
                        TurnOFF_Device = xml.ReadInnerXml
                        reach = 1
                    End If

                    If reach = 1 Then
                        sqlquery.CommandText = "Update UserAction set TurnON_Devices= '" & TurnON_Device & "' where ScenarioNo = 'Scenario_" & TotalScenario.ToString & "' And XMLName = '" & filename & "'"
                        sqlquery.ExecuteNonQuery()

                        sqlquery.CommandText = "Update UserAction set TurnOFF_Devices= '" & TurnOFF_Device & "' where ScenarioNo = 'Scenario_" & TotalScenario.ToString & "' And XMLName = '" & filename & "'"
                        sqlquery.ExecuteNonQuery()
                        TurnON_Device = ""
                        TurnOFF_Device = ""
                        CheckDevice = {""}
                        cnt = 0
                        TotalScenario += 1
                        If TotalScenario <= total Then
                            sqlquery.CommandText = "INSERT INTO UserAction(ScenarioNo,BaseName,Environment,UserName,UserAction,Location,Timing,TurnON_Devices,TurnOFF_Devices,XMLName)VALUES('Scenario_" & TotalScenario.ToString & "','" & BaseName & "','" & Envname & "','','','','','','','" & filename & "');"
                            sqlquery.ExecuteNonQuery()
                        End If
                        reach = 0
                    End If
                End If
            End While
            TotalScenario = 1
            reach = 0
        Next (File)
        Dim SplitString() As String = {""}

        sqlquery.CommandText = "select * from UserAction"
        sqlquery.ExecuteNonQuery()
        dr = sqlquery.ExecuteReader
        While dr.Read
            ReDim Preserve SplitString(arrlen)
            SplitString(arrlen) = dr("TurnON_Devices").ToString + "," + dr("UserName").ToString
            arrlen += 1
        End While
        dr.Close()

        sqlquery.CommandText = "select * from TotalDevice_1"
        dr = sqlquery.ExecuteReader
        While dr.Read
            ReDim Preserve DeviceList(arrlen1)
            DeviceList(arrlen1) = dr("DeviceList")
            arrlen1 += 1
        End While
        dr.Close()


        Dim Restrictions() As String = {Nothing, Nothing, "Total_User", Nothing}
        Dim CollectionName As String = "Columns"

        For i As Integer = 0 To arrlen1 - 1
            If DeviceList(i) <> "Room" And DeviceList(i) <> "user" Then
                Dim dt As DataTable = sqlconn.GetSchema(CollectionName, Restrictions)
                For Each TableRow As DataRow In dt.Rows
                    If TableRow.Item("COLUMN_NAME") = UCase(DeviceList(i)) Then
                        sqlquery.CommandText = "Alter Table Total_User Drop column " & DeviceList(i) & " "
                        sqlquery.ExecuteNonQuery()
                    End If
                Next
                sqlquery.CommandText = "Alter Table Total_User add column " & UCase(DeviceList(i)) & " Number "
                sqlquery.ExecuteNonQuery()
                sqlquery.CommandText = "Update Total_User set " & UCase(DeviceList(i)) & "=0"
                sqlquery.ExecuteNonQuery()
            End If
        Next

        For i As Integer = 0 To arrlen - 1
            DeviceList1 = SplitString(i).Split(","c)
            'ComboBox1.Items.Add(LCase(DeviceList1(DeviceList1.Length - 1)))
            For j As Integer = 0 To DeviceList1.Length - 2
                For k As Integer = 0 To DeviceList.Length - 1
                    If DeviceList1(j).Contains(DeviceList(k)) Then
                        sqlquery.CommandText = "Update Total_User set " & DeviceList(k) & "= " & DeviceList(k) & "+1 where UserName ='" & DeviceList1(DeviceList1.Length - 1) & "'"
                        sqlquery.ExecuteNonQuery()
                    End If
                Next
            Next
        Next


    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim User_Name As String = ""
        If ComboBox1.SelectedItem = "" Then
            MessageBox.Show("Please Select The User")
        Else

            Chart1.Series(0).Points.Clear()
            User_Name = ComboBox1.SelectedItem

            If flag = 0 Then
                sqlquery.CommandText = "Select * from Total_User where UserName ='" & User_Name & "'"
                dr = sqlquery.ExecuteReader

                dr.Read()
                For i As Integer = 0 To DeviceList.Length - 1
                    If DeviceList(i) <> "Room" And DeviceList(i) <> "user" Then
                        Chart1.Series(0).Points.AddXY(UCase(DeviceList(i)), dr(DeviceList(i)).ToString)
                    End If
                Next
                dr.Close()
                flag = 1
            End If

        End If
        flag = 0
    End Sub
    
End Class