Imports System.IO
Imports System.Xml
Public Class FreqDevice
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
    Dim s() As String = {""}
    Public arrlen As Integer = 0
    Public arrlen1 As Integer = 0
    Private Sub btnclose(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.FormClosing
        ChooseChart.Show()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
        ChooseChart.Show()
    End Sub
    Private Sub FreqDevice_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
            ReDim Preserve DeviceList1(arrlen1)
            DeviceList(arrlen1) = dr("DeviceList")
            DeviceList1(arrlen1) = DeviceList(arrlen1)
            arrlen1 += 1
        End While
        dr.Close()

        For i As Integer = 0 To arrlen - 1
            s = SplitString(i).Split(","c)
            For j As Integer = 0 To s.Length - 2
                For k As Integer = 0 To arrlen1 - 1
                    If s(j).Contains(DeviceList(k)) Then
                        DeviceList1(k) = s(s.Length - 1) + "," + DeviceList1(k)
                    End If
                Next
            Next
        Next

        For k As Integer = 0 To arrlen1 - 1
            s = DeviceList1(k).Split(","c)
            sqlquery.CommandText = "update TotalDevice_1 set Overall_User = '" & (s.Length - 1).ToString & "' where DeviceList='" & s(s.Length - 1) & "'"
            sqlquery.ExecuteNonQuery()
        Next
        
    End Sub

   
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        sqlquery.CommandText = "Select * from TotalDevice_1"
        dr = sqlquery.ExecuteReader

        While (dr.Read)
            If dr("DeviceList") <> "Room" And dr("DeviceList") <> "user" Then
                Chart1.Series(0).Points.AddXY(UCase(dr("DeviceList")), dr("Overall_User").ToString)
            End If
        End While
        dr.Close()
    End Sub

End Class