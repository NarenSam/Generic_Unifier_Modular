Imports System.Data.OleDb
Imports System.ComponentModel
Imports System.Reflection
Imports System.Drawing.Imaging

Public Class Form3
    Public priority As String = ""
    Public indicate As Integer = 0
    Dim dragging As Boolean = False
    Public beginX, beginY As Integer
    Public send As Object
    Public substrings() As String
    Public roomArray() As roo
    Dim fd As OpenFileDialog = New OpenFileDialog()
    Dim oObject As System.Object
    Dim ThisAssembly As Assembly
    Dim m_MySteps As Integer
    Dim name_class As String
    Public devicecount() As Integer = {0}
    Public id As Integer = 1
    Public id1 As Integer = 1
    Public pos As Integer = 44
    Public cnt As Integer = 0
    Public sqlconn As New OleDb.OleDbConnection
    Public connString As String
    Public sqlquery As New OleDb.OleDbCommand
    Public flag As Integer = 0
    Public dr As OleDb.OleDbDataReader
    Public OpenFileDlg As New OpenFileDialog
    Public username As String

    Public tid1 As Integer = 1
    Public n As Integer = 1
    Public n20 As Integer = 0
    Public envname As String
    Private Sub Panel2_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel2.Paint
        TextBox1.Text = Panel2.Width
        TextBox2.Text = Panel2.Height
    End Sub
    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.Close()
        Form2.Show()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        AboutBox1.Show()
    End Sub
    Private Sub NewDeviceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewDeviceToolStripMenuItem.Click
        If Label2.Text <> "Untitled" Then
            StartScenario.Show()
        Else
            MessageBox.Show("Please save the Environment before running it.")
        End If
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Form1.Close()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        Form5.Show()
    End Sub

    Private Sub btnclose(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.FormClosing
        Me.Hide()
        Base.Show()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Panel2.Width = Convert.ToString(Convert.ToInt32(TextBox1.Text) * 10)
        Panel2.Height = Convert.ToString(Convert.ToInt32(TextBox2.Text) * 10)

    End Sub
    Private Sub Panel2_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel2.MouseDoubleClick
        Form7.Show()
    End Sub
    Private Sub PropertyGrid1_PropertyValueChanged(ByVal s As Object, ByVal e As System.Windows.Forms.PropertyValueChangedEventArgs) Handles PropertyGrid1.PropertyValueChanged
        check()
    End Sub


    Private Sub Room1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Room.MouseDoubleClick
        Addroom()
        n20 = n20 + 1
    End Sub

    Public Sub Addroom()
        ReDim Preserve roomArray(n20)
        roomArray(n20) = New roo
        With (roomArray(n20))
            .Width = 100
            .Height = 100
            .Sname = "Room" & n20
            .Smartid = id1
            .Slocation = "Environment"
            id1 += 1

            Panel2.Controls.Add(roomArray(n20))
            roomArray(n20).BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            AddHandler .MouseDown, AddressOf Me.MoveDown
            AddHandler .MouseMove, AddressOf Me.MoveDevice
            AddHandler .MouseUp, AddressOf Me.MoveUp
            AddHandler .MouseClick, AddressOf Me.Propertygridvalues
        End With
    End Sub


    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Form5.Show()
        Me.Close()
    End Sub


    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
        Form2.Show()
    End Sub

    Private Sub BuildScenarioToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BuildScenarioToolStripMenuItem.Click
        If Label2.Text <> "Untitled" Then
            TakeScreenShot(Panel2).Save(System.Environment.CurrentDirectory + "\Environment_Image\" + Label2.Text + ".png", System.Drawing.Imaging.ImageFormat.Png)
            input.Show()
        Else
            MessageBox.Show("Please save the Environment before running it.")
        End If
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripMenuItem.Click
        id1 -= 1
        send.Dispose()
        send.Refresh()
    End Sub

    Private Sub CreateDeviceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreateDeviceToolStripMenuItem.Click
        DeviceCreation1.Show()
        Me.Hide()
    End Sub
    Private Sub RefreshToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshToolStripMenuItem.Click
        sqlconn.Close()
        indicate = 1
        id1 = 1
        Me.Panel2.Controls.Clear()
        Base.OpenToolStripMenuItem1_Click(e, e)

    End Sub
    Public Sub Form3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.Environment.CurrentDirectory & "\Device.mdb"
        sqlconn.ConnectionString = connString
        sqlquery.Connection = sqlconn
        sqlconn.Open()
        sqlquery.CommandText = "select * from DeviceNS_1"
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
                Panel1.Controls.Add(oObject)

                With oObject
                    .Smartid = id
                    .Sname = dr("DeviceName")
                    .SLocation = "Panel1"
                    .width = dr("Width")
                    .height = dr("Height")
                    .Location = New Point(dr("LocationX"), dr("LocationY"))
                    oObject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                    AddHandler DirectCast(oObject, Control).MouseClick, AddressOf Me.Propertygridvalues
                    AddHandler DirectCast(oObject, Control).MouseDoubleClick, AddressOf Me.AddDevice
                End With
                ToolTip1.SetToolTip(oObject, oObject.Sname + " Device")
                id = id + 1
            End If
        End While
        ReDim Preserve devicecount(id)
        dr.Close()
        id = 1
    End Sub
    Private Sub UserPriority(ByVal sender As Object)
        Dim replicate As Integer = 0

        username = InputBox("Set a  Unique Name for the User")
        For Each ctrl As Control In Panel2.Controls
            If String.Compare(CType(ctrl, Object).Sname, username) = 0 Then
                MessageBox.Show("User Already Exist in Panel")
                replicate = 1
            End If
        Next
        If replicate = 0 Then
            If username <> "" And username <> "" Then
                sqlquery.CommandText = "select * from Total_User"
                dr = sqlquery.ExecuteReader
                While dr.Read
                    If dr("UserName").ToString = UCase(username) Then
                        sender.Sname = dr("UserName")
                        sender.Priority = dr("Priority")
                        flag = 1
                        creation(sender)
                    End If
                End While
                If flag = 0 Then
                    MessageBox.Show("Unknown User")
                End If
                flag = 0
            Else
                MessageBox.Show("Please Enter Proper Name...!!!")
            End If
            dr.Close()
        End If
    End Sub
    Public Sub AddDevice(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        dragging = False
        If sender.ToString = "user.user" Then
            UserPriority(sender)
        Else
            If sender.ToString <> "Gum.roo" Then
                creation(sender)
            End If
        End If

    End Sub
    Private Sub creation(ByVal sender As Object)
        Dim n As Integer = 1
        For Each ctrl As Control In Panel2.Controls

            If String.Compare(ctrl.ToString, sender.ToString) = 0 Then
                n = n + 1
            End If

        Next
        If sender.ToString = "user.user" Then
            OpenFileDlg.FileName = (System.Environment.CurrentDirectory + "\Environment_Device\user.dll")
        Else
            OpenFileDlg.FileName = (System.Environment.CurrentDirectory + "\Environment_Device\" + sender.Sname + ".dll")
        End If

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
        Panel2.Controls.Add(oObject)

        With oObject
            .Smartid = id1
            If sender.ToString = "user.user" Then
                .Sname = username
            Else
                .Sname = sender.Sname + n.ToString
            End If

            .SLocation = "Environment"
            .width = 47
            .height = 47
            .Location = New Point(10, pos)
            If oObject.ToString = "user.user" Then
                .Priority = sender.Priority
            End If
            AddHandler DirectCast(oObject, Control).MouseDown, AddressOf Me.MoveDown
            AddHandler DirectCast(oObject, Control).MouseMove, AddressOf Me.MoveDevice
            AddHandler DirectCast(oObject, Control).MouseUp, AddressOf Me.MoveUp
            AddHandler DirectCast(oObject, Control).MouseClick, AddressOf Me.clickstatus
        End With
        ToolTip1.SetToolTip(oObject, oObject.Sname)
        n = 0
        id1 = id1 + 1

    End Sub
    Public Sub MoveDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Dim btn As Object = sender
        PropertyGrid1.SelectedObject = btn

        dragging = True
        beginX = e.X
        beginY = e.Y
        If btn.ToString <> "GUM.roo" Then
            btn.BringToFront()
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

    Public Sub MoveUp(ByVal sender As Object, ByVal e As MouseEventArgs)
        dragging = False
        check()
        If sender.ToString <> "GUM.roo" Then
            PropertyGrid1.Refresh()
        End If
    End Sub
    Public Sub clickstatus(ByVal sender As Object, ByVal e As MouseEventArgs)
        send = sender

    End Sub
    Public Sub MoveDevice(ByVal sender As Object, ByVal e As MouseEventArgs)

        If dragging = True Then
            If Panel2.ClientRectangle.Contains(New Rectangle(New Point(sender.Location.X + e.X - beginX, sender.Location.Y + e.Y - beginY), sender.Size)) Then
                sender.Location = New Point(sender.Location.X + e.X - beginX, sender.Location.Y + e.Y - beginY)
            End If
        End If
    End Sub

    Public Sub check()
        Dim btn As Object
        Dim c = 0
        For Each btn In Panel2.Controls

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
            c = 0
        Next
    End Sub

    Public Function TakeScreenShot(ByVal Control As Control) As Bitmap
        Dim tmpImg As New Bitmap(Control.Width, Control.Height)
        Using g As Graphics = Graphics.FromImage(tmpImg)
            g.CopyFromScreen(Panel2.PointToScreen(New Point(0, 0)), New Point(0, 0), New Size(Panel2.Width, Panel2.Height))
        End Using
        Return tmpImg
    End Function

    Public Sub SaveAsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsToolStripMenuItem.Click
        Dim id2 As Integer = 1
        Dim str As String
        Dim o As Object

        TakeScreenShot(Panel2).Save(System.Environment.CurrentDirectory + "\Environment_Device\" + Label2.Text + ".png", System.Drawing.Imaging.ImageFormat.Png)
        sqlquery.CommandText = "delete * from EnvironmentDevice where Environment='" & Label2.Text & "' and BaseName ='" & Base.Label2.Text & "'"
        sqlquery.ExecuteNonQuery()
        For Each ctrl As Control In Panel2.Controls
            If ctrl.ToString = "user.user" Then
                priority = CType(ctrl, Object).Priority
            Else
                priority = ""
            End If
            If ctrl.ToString <> "GUM.roo" Then
                str = ctrl.ToString
                o = ctrl
                substrings = str.Split("."c)
                If ctrl.Visible = "True" Then
                    sqlquery.CommandText = "INSERT INTO EnvironmentDevice(DeviceNumber,Smartid,Sname,Slocation,Status,DeviceName,DeviceClass,BaseName,Environment,LocationX,LocationY,Width,Height,Ewidth,Eheight,UserPriority)VALUES('" & id2.ToString & "','" & o.Smartid.ToString & "','" & o.Sname.ToString & "','" & o.Slocation.ToString & "','" & o.Status.ToString & "','" & substrings(0) & "','" & ctrl.ToString & "','" & Base.Label2.Text & "','" & Label2.Text & "','" & ctrl.Location.X & "','" & ctrl.Location.Y & "','" & ctrl.Width & "','" & ctrl.Height & "','" & Panel2.Width & "','" & Panel2.Height & "','" & priority & "');"
                    sqlquery.ExecuteNonQuery()
                    id2 = id2 + 1
                End If
            End If

        Next
        If Not roomArray Is Nothing Then
            For i As Integer = 0 To roomArray.Length - 1
                If roomArray(i).Visible = "True" Then
                    sqlquery.CommandText = "INSERT INTO EnvironmentDevice(DeviceNumber,Smartid,Sname,Slocation,Status,DeviceName,DeviceClass,BaseName,Environment,LocationX,LocationY,Width,Height,Ewidth,Eheight)VALUES('" & id2.ToString & "','" & roomArray(i).Smartid & "','" & roomArray(i).Sname & "','" & roomArray(i).Slocation & "','False','Room','GUM.roo','" & Base.Label2.Text & "','" & Label2.Text & "','" & roomArray(i).Location.X & "','" & roomArray(i).Location.Y & "','" & roomArray(i).Width & "','" & roomArray(i).Height & "','" & Panel2.Width & "','" & Panel2.Height & "');"
                    sqlquery.ExecuteNonQuery()
                    id2 = id2 + 1
                End If
            Next i
        End If
        priority = ""
        MessageBox.Show("Environment Updated")
    End Sub

    Private Sub CreateUserToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreateUserToolStripMenuItem.Click
        Me.Hide()
        UserCreation.Show()
    End Sub

   
End Class

Public Class roo
    Inherits Room.Room
    Protected Overrides ReadOnly Property CreateParams As System.Windows.Forms.CreateParams
        Get
            Dim CP As CreateParams = MyBase.CreateParams
            CP.Style = CP.Style Or &H40000
            Return CP
        End Get
    End Property
End Class

