Imports System.Data.OleDb
Imports System.Xml
Imports System.ComponentModel
Imports System.Reflection
Public Class StartScenario

    Public a As Integer = 9999
    Public b As String = ""
    Public TotalScenario As Integer = 1
    Public id As Integer = 1
    Public reach As Integer = 0
    Public sqlconn As New OleDb.OleDbConnection
    Public connString As String
    Public sqlquery As New OleDb.OleDbCommand
    Public flag As Integer = 0
    Public flag1 As Integer = 0
    Public dr As OleDb.OleDbDataReader
    Public OpenFileDlg As New OpenFileDialog
    Dim oObject As System.Object
    Dim ThisAssembly As Assembly
    Public Total As Integer = 0
    Public Device() As String

    Public CheckDevice() As String
    Public timeflag As Integer = 0
    Public cnt As Integer = 0
    Public roomArray() As roo
    Public ScenarioArray() As String
    Public n As Integer = 0
    Dim filename As String
    Dim dragging As Boolean
    Public beginX, beginY As Integer
    Dim name_class As String
    Public devicecount() As Integer = {0}
    Dim TurnON_Device As String
    Dim TurnOFF_Device As String
    Dim BaseName As String
    Dim Envname As String
    Public count As Integer = 0

    Public username() As String = {""}
    Public useraction() As String = {""}
    Public userlocation() As String = {""}
    Public timing() As String = {""}
    Public turnon() As String = {""}
    Public turnoff() As String = {""}

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        
        With OpenFileDialog1
            .Title = "Open Scenario"
            .Filter = "XML Files (.xml)|*.xml"
            .InitialDirectory = Application.StartupPath + "\XML_Files\"
            .RestoreDirectory = True
            .FileName = "Open xml"
        End With
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            flag = 0
            filename = System.IO.Path.GetFullPath(OpenFileDialog1.FileName)

            sqlquery.CommandText = "select * from XML_File where XMLName ='" & System.IO.Path.GetFileName(OpenFileDialog1.FileName) & "'"
            sqlquery.ExecuteNonQuery()
            dr = sqlquery.ExecuteReader
            dr.Read()
            Total = dr("TotalScenario")
            dr.Close()

            ReDim Preserve username(Total)
            ReDim Preserve useraction(Total)
            ReDim Preserve userlocation(Total)
            ReDim Preserve timing(Total)
            ReDim Preserve turnon(Total)
            ReDim Preserve turnoff(Total)
            sqlquery.CommandText = "select * from UserAction"
            sqlquery.ExecuteNonQuery()
            dr = sqlquery.ExecuteReader

            While dr.Read
                username(count) = dr("UserName")
                useraction(count) = dr("UserAction")
                userlocation(count) = dr("Location")
                timing(count) = dr("Timing")
                If Not IsDBNull(dr("TurnON_Devices")) Then
                    turnon(count) = dr("TurnON_Devices")
                End If
                If Not IsDBNull(dr("TurnOFF_Devices")) Then
                    turnoff(count) = dr("TurnOFF_Devices")
                End If
                count += 1
            End While
            dr.Close()

            Dim xml As New Xml.XmlTextReader(filename)
            While xml.Read

                If xml.NodeType = XmlNodeType.Element Then

                    If xml.Name = "Base" Then
                        If flag = 0 Then
                            sqlquery.CommandText = "Delete * from UserAction"
                            sqlquery.ExecuteNonQuery()
                        End If
                        BaseName = xml.ReadInnerXml

                        sqlquery.CommandText = "INSERT INTO UserAction(ScenarioNo,BaseName,Environment,UserName,UserAction,Location,Timing,TurnON_Devices,TurnOFF_Devices)VALUES('','" & BaseName & "','','','','','','','');"
                        sqlquery.ExecuteNonQuery()

                    End If
                    If xml.Name = "Environment" Then
                        Envname = xml.ReadInnerXml
                        If Envname = Label2.Text Then
                            sqlquery.CommandText = "Update UserAction set Environment = '" & Envname & "' "
                            sqlquery.ExecuteNonQuery()
                            MessageBox.Show("Scenario Loaded")
                        Else
                            MessageBox.Show("Xml Scenario Not Suited")
                        End If
                    End If

                    If xml.Name = "Scenario_" + TotalScenario.ToString And TotalScenario <= 1 Then
                        sqlquery.CommandText = "Update UserAction set ScenarioNo = '" & xml.Name & "'"

                        sqlquery.ExecuteNonQuery()
                    End If


                    If xml.Name = "User" Then
                        sqlquery.CommandText = "Update UserAction set UserName = '" & xml.ReadInnerXml.ToString & "' where ScenarioNo = 'Scenario_" & TotalScenario.ToString & "' "
                        sqlquery.ExecuteNonQuery()
                    End If
                    If xml.Name = "Action" Then
                        sqlquery.CommandText = "Update UserAction set UserAction = '" & xml.ReadInnerXml.ToString & "' where ScenarioNo = 'Scenario_" & TotalScenario.ToString & "' "
                        sqlquery.ExecuteNonQuery()
                    End If
                    If xml.Name = "Location" Then
                        sqlquery.CommandText = "Update UserAction set Location = '" & xml.ReadInnerXml.ToString & "' where ScenarioNo = 'Scenario_" & TotalScenario.ToString & "' "
                        sqlquery.ExecuteNonQuery()
                    End If

                    If xml.Name = "Time" Then
                        sqlquery.CommandText = "Update UserAction set Timing = '" & xml.ReadInnerXml.ToString & "' where ScenarioNo = 'Scenario_" & TotalScenario.ToString & "'"
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
                        sqlquery.CommandText = "Update UserAction set TurnON_Devices= '" & TurnON_Device & "' where ScenarioNo = 'Scenario_" & TotalScenario.ToString & "'"
                        sqlquery.ExecuteNonQuery()

                        sqlquery.CommandText = "Update UserAction set TurnOFF_Devices= '" & TurnOFF_Device & "' where ScenarioNo = 'Scenario_" & TotalScenario.ToString & "'"
                        sqlquery.ExecuteNonQuery()
                        TurnON_Device = ""
                        TurnOFF_Device = ""
                        CheckDevice = {""}
                        cnt = 0

                        TotalScenario += 1
                        If TotalScenario <= Total Then
                            sqlquery.CommandText = "INSERT INTO UserAction(ScenarioNo,BaseName,Environment,UserName,UserAction,Location,Timing,TurnON_Devices,TurnOFF_Devices)VALUES('Scenario_" & TotalScenario.ToString & "','" & BaseName & "','" & Envname & "','','','','','','');"
                            sqlquery.ExecuteNonQuery()
                            flag = 1
                        End If
                        reach = 0
                    End If
                End If
            End While

        End If
    End Sub

    Private Sub btnclose(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.FormClosing
        Me.Hide()
    End Sub
    Private Sub StartScenario_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim n20 As Integer = 0
        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.Environment.CurrentDirectory & "\Device.mdb"
        sqlconn.ConnectionString = connString
        sqlquery.Connection = sqlconn
        sqlconn.Open()
        Label2.Text = Form3.Label2.Text
        'Label2.Text = "Hospital1"
        'Label2.Text = "SmartHome1"
        Me.WindowState = FormWindowState.Maximized

        

        sqlquery.CommandText = "SELECT * FROM EnvironmentDevice where Environment='" & Form3.Label2.Text & "' and BaseName='" & Base.Label2.Text & "'"
        'sqlquery.CommandText = "SELECT * FROM EnvironmentDevice where Environment='Hospital1' and BaseName= 'SmartBase'"
        'sqlquery.CommandText = "SELECT * FROM EnvironmentDevice where Environment='SmartHome1' and BaseName= 'SmartBase'"
        dr = sqlquery.ExecuteReader
        While dr.Read
            If System.IO.File.Exists(System.Environment.CurrentDirectory + "\Environment_Device\" + dr("DeviceName") + ".dll") = True And dr("DeviceName") <> "Room" Then
                OpenFileDlg.FileName = (System.Environment.CurrentDirectory + "\Environment_Device\" + dr("DeviceName") + ".dll")
                ThisAssembly = Assembly.LoadFrom(OpenFileDlg.FileName)
                Dim TypeObj As Type
                For Each TypeObj In ThisAssembly.GetTypes()
                    Dim t As Integer = 0
                    Dim iteration As Integer = 0
                    For Each c As Char In TypeObj.ToString
                        If c = "." Then
                            t = t + 1
                        End If
                    Next
                    If t = 1 Then
                        name_class = TypeObj.ToString
                        Exit For
                    Else
                        t = 0
                    End If
                Next
                TypeObj = ThisAssembly.GetType(name_class)
                oObject = Activator.CreateInstance(TypeObj)
                Panel2.Width = dr("Ewidth")
                Panel2.Height = dr("Eheight")

                Panel2.Controls.Add(oObject)
                With oObject
                    .Smartid = dr("Smartid")
                    .Sname = dr("Sname")
                    .SLocation = dr("Slocation")
                    .width = dr("Width")
                    .height = dr("Height")
                    If oObject.ToString = "user.user" Then
                        .Priority = dr("UserPriority")
                    End If
                    .Location = New Point(dr("LocationX"), dr("LocationY"))
                    AddHandler DirectCast(oObject, Control).MouseDown, AddressOf Me.MoveDown
                    AddHandler DirectCast(oObject, Control).MouseUp, AddressOf Me.MoveUp
                    AddHandler DirectCast(oObject, Control).MouseDown, AddressOf Me.Propertygridvalues
                    AddHandler DirectCast(oObject, Control).MouseDoubleClick, AddressOf Me.clickStatus
                    If oObject.ToString = "user.user" Then
                        AddHandler DirectCast(oObject, Control).MouseMove, AddressOf Me.MMoveUser

                        .BringToFront()
                    End If
                End With

            Else
                If dr("DeviceName") = "Room" Then
                    ReDim Preserve roomArray(n20)
                    roomArray(n20) = New roo

                    With (roomArray(n20))
                        .Width = dr("Width")
                        .Height = dr("Height")
                        .Sname = dr("Sname")
                        .Slocation = dr("Slocation")
                        .Smartid = dr("Smartid")

                        Panel2.Controls.Add(roomArray(n20))
                        roomArray(n20).BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
                        AddHandler .MouseClick, AddressOf Me.Propertygridvalues
                        .SendToBack()
                    End With
                    roomArray(n20).Location = New Point(dr("LocationX"), dr("LocationY"))
                    n20 += 1

                End If
            End If

        End While
        dr.Close()
    End Sub
    Public Sub MoveDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Dim btn As Object = sender
        PropertyGrid1.SelectedObject = btn
        dragging = True
        beginX = e.X
        beginY = e.Y
        btn.BringToFront()

    End Sub

    Public Sub MoveUp(ByVal sender As Object, ByVal e As MouseEventArgs)
        dragging = False
        check(sender)
        PropertyGrid1.Refresh()
        CheckScenario(sender)
    End Sub
    Public Sub CheckScenario(ByVal sender As Object)

        Dim time As String

        Dim hours As Integer = DateTime.Now.Hour
        Dim daypart As String = ""

        sqlquery.CommandText = "select * from UserAction"
        sqlquery.ExecuteNonQuery()
        dr = sqlquery.ExecuteReader
        dr.Read()
        time = dr("Timing")
        'time = "Afternoon"
        If hours < 12 Then
            daypart = "Morning"
        ElseIf hours <= 17 Then
            daypart = "Afternoon"
        ElseIf hours <= 19 Then
            daypart = "Evening"
        ElseIf hours <= 23 Then
            daypart = "Night"
        End If
        If time = daypart Or time = "Any Time" Then
            timeflag = 1
        End If

        dr.Close()
        sqlquery.CommandText = "select * from UserAction"
        sqlquery.ExecuteNonQuery()
        dr = sqlquery.ExecuteReader
        For i As Integer = 0 To Total - 1
            If sender.ToString = "user.user" Then
                If sender.Sname = username(i) Then
                    If sender.Action = useraction(i) Then
                        If sender.Slocation = userlocation(i) Then
                            If timeflag = 1 Then
                                If turnon(i) <> "" Then
                                    Device = turnon(i).ToString.Split(","c)
                                    For j As Integer = 0 To Device.Length - 1
                                        For Each ctrl As Control In Panel2.Controls
                                            If CType(ctrl, Object).Sname.ToString = Device(j) Then
                                                CType(ctrl, Object).Status = True
                                                ctrl.BackgroundImage = System.Drawing.Image.FromFile(ctrl.ToString & "1.png")
                                                flag1 = 1
                                            End If
                                        Next
                                    Next

                                End If
                                Device = {""}
                                If turnoff(i) <> "" Then
                                    Device = turnoff(i).ToString.Split(","c)
                                    For j As Integer = 0 To Device.Length - 1
                                        For Each ctrl As Control In Panel2.Controls
                                            If CType(ctrl, Object).Sname.ToString = Device(j) Then
                                                CType(ctrl, Object).Status = False
                                                ctrl.BackgroundImage = System.Drawing.Image.FromFile(ctrl.ToString & ".png")
                                                flag1 = 1
                                            End If
                                        Next
                                    Next
                                End If
                            Else
                                If flag1 = 1 Then
                                    Revert(i)
                                End If
                            End If
                        Else
                            If flag1 = 1 Then
                                Revert(i)
                            End If
                        End If
                    Else
                        If flag1 = 1 Then
                            Revert(i)
                        End If
                    End If
                End If
            End If
        Next
        timeflag = 0
        dr.Close()

    End Sub
    Public Sub Revert(ByVal i As Integer)
        If turnon(i) <> "" Then
            Device = turnon(i).ToString.Split(","c)

            For j As Integer = 0 To Device.Length - 1

                For Each ctrl As Control In Panel2.Controls
                    If CType(ctrl, Object).Sname.ToString = Device(j) Then
                        ctrl.BackgroundImage = System.Drawing.Image.FromFile(ctrl.ToString & ".png")
                        CType(ctrl, Object).Status = False
                        flag1 = 1
                    End If
                Next
            Next
        End If

        Device = {""}
        If turnoff(i) <> "" Then
            Device = turnoff(i).ToString.Split(","c)
            For j As Integer = 0 To Device.Length - 1
                For Each ctrl As Control In Panel2.Controls
                    If CType(ctrl, Object).Sname.ToString = Device(j) Then
                        ctrl.BackgroundImage = System.Drawing.Image.FromFile(ctrl.ToString & "1.png")
                        CType(ctrl, Object).Status = True
                        flag1 = 1
                    End If
                Next
            Next
        End If
    End Sub
    Public Sub MMoveUser(ByVal sender As Object, ByVal e As MouseEventArgs)
        If dragging = True Then
            If Panel2.ClientRectangle.Contains(New Rectangle(New Point(sender.Location.X + e.X - beginX, sender.Location.Y + e.Y - beginY), sender.Size)) Then
                sender.Location = New Point(sender.Location.X + e.X - beginX, sender.Location.Y + e.Y - beginY)
            End If
        End If
    End Sub
    Public Sub Propertygridvalues(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        PropertyGrid1.SelectedObject = sender
        dragging = True
        beginX = e.X
        beginY = e.Y
        If sender.ToString <> "GUM.roo" Then
            sender.BringToFront()
        End If
    End Sub
    Public Sub check(ByVal sender As Object)
        Dim btn As Object
        Dim c As Integer = 0
        btn = sender

        If Not roomArray Is Nothing And btn.ToString <> "GUM.roo" Then
            For i As Integer = 0 To roomArray.Length - 1
                If btn.Location.X >= roomArray(i).Location.X And btn.Location.X <= (roomArray(i).Location.X + roomArray(i).Width) And btn.Location.Y >= roomArray(i).Location.Y And btn.Location.Y <= (roomArray(i).Location.Y + roomArray(i).Height) Then
                    btn.Slocation = roomArray(i).Sname
                    c = 1
                End If
            Next
        End If
        If c = 0 Then
            btn.Slocation = "Environment"
        End If
    End Sub


    Public Sub clickStatus(ByVal sender As Object, ByVal e As MouseEventArgs)
        If sender.ToString <> "user.user" Then
            If sender.Status = True Then
                sender.Status = False
                sender.BackgroundImage = System.Drawing.Image.FromFile(sender.ToString & ".png")
            Else
                sender.Status = True
                sender.BackgroundImage = System.Drawing.Image.FromFile(sender.ToString & "1.png")
            End If
        End If
    End Sub

    Private Sub ExitToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem1.Click
        Form1.Close()
    End Sub
End Class