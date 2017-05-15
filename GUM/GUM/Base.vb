Imports System.Data.OleDb
Imports System.ComponentModel
Imports System.Reflection
Imports System.Drawing.Imaging
Public Class Base
    Public indicate As Integer = 0
    Public substrings() As String
    Dim dragging As Boolean
    Public beginX, beginY As Integer
    Public send As Object
    Dim fd As OpenFileDialog = New OpenFileDialog()
    Dim oObject As System.Object
    Dim ThisAssembly As Assembly
    Dim m_MySteps As Integer
    Dim name_class As String
    Public devicecount() As Integer = {0}
    Public id As Integer = 1
    Public id1 As Integer = 1
    Public pos As Integer = 10
    Public cnt As Integer = 0
    Public basename As String
    Public envname As String
    Dim Setting As Integer = 0
    Public sqlconn As New OleDb.OleDbConnection
    Public connString As String
    Public sqlquery As New OleDb.OleDbCommand
    Public flag As Integer = 0
    Public dr As OleDb.OleDbDataReader
    Public OpenFileDlg As New OpenFileDialog
    Dim layoutname As String
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Form1.Close()
    End Sub

    Private Sub btnclose(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.FormClosing
        Form2.Show()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.Close()
        Form2.Show()
    End Sub
    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripMenuItem.Click
        send.Dispose()
        send.Refresh()
    End Sub

    Public Sub OpenToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem1.Click
        Dim n20 As Integer = 0
        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.Environment.CurrentDirectory & "\Device.mdb"
        sqlconn.ConnectionString = connString
        sqlquery.Connection = sqlconn
        sqlconn.Open()

        If Label2.Text <> "Untitled" Then
            Form3.Label2.Text = envname
            sqlquery.CommandText = "SELECT * FROM EnvironmentDevice where Environment='" & envname & "' and BaseName='" & Label2.Text & "'"

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
                    Form3.Panel2.Width = dr("Ewidth")
                    Form3.Panel2.Height = dr("Eheight")

                    Form3.Panel2.Controls.Add(oObject)
                    With oObject
                        .Smartid = dr("Smartid")
                        .Sname = dr("Sname")
                        .SLocation = dr("Slocation")
                        .width = dr("Width")
                        .height = dr("Height")
                        .Location = New Point(dr("LocationX"), dr("LocationY"))
                        AddHandler DirectCast(oObject, Control).MouseDown, AddressOf Form3.Propertygridvalues
                        AddHandler DirectCast(oObject, Control).MouseMove, AddressOf Form3.MoveDevice
                        AddHandler DirectCast(oObject, Control).MouseUp, AddressOf Form3.MoveUp
                        AddHandler DirectCast(oObject, Control).MouseClick, AddressOf Form3.clickstatus
                        .BringToFront()
                    End With
                    ToolTip1.SetToolTip(oObject, oObject.Sname)
                    Form3.id1 = Form3.id1 + 1

                Else
                    If dr("DeviceName") = "Room" Then
                        ReDim Preserve Form3.roomArray(n20)
                        Form3.roomArray(n20) = New roo
                        With (Form3.roomArray(n20))
                            .Width = dr("Width")
                            .Height = dr("Height")
                            .Sname = dr("Sname")
                            .Slocation = dr("Slocation")
                            .Smartid = dr("Smartid")
                            Form3.Panel2.Controls.Add(Form3.roomArray(n20))
                            Form3.roomArray(n20).BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
                            AddHandler .MouseDown, AddressOf Form3.MoveDown
                            AddHandler .MouseMove, AddressOf Form3.MoveDevice
                            AddHandler .MouseUp, AddressOf Form3.MoveUp
                            AddHandler .MouseClick, AddressOf Form3.Propertygridvalues
                            .SendToBack()
                        End With
                        Form3.roomArray(n20).Location = New Point(dr("LocationX"), dr("LocationY"))
                        n20 += 1
                        Form3.id1 = Form3.id1 + 1
                    End If
                End If

            End While
            Form3.n20 = n20
            dr.Close()
            sqlconn.Close()
            If Form3.indicate = 1 Then
                Form3.Form3_Load(e, e)
            Else
                Form3.Show()
            End If
            Me.Hide()
        Else
            MessageBox.Show("Please Save the Base and Continue!!!")
        End If

        sqlconn.Close()
    End Sub


    Private Sub Panel2_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel2.MouseDoubleClick
        Form7.Show()
    End Sub

    Public Sub Base_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.Environment.CurrentDirectory & "\Device.mdb"
        sqlconn.ConnectionString = connString
        sqlquery.Connection = sqlconn
        sqlconn.Open()
        sqlquery.CommandText = "select * from DeviceNS"
        dr = sqlquery.ExecuteReader

        While dr.Read
            If System.IO.File.Exists(System.Environment.CurrentDirectory + "\Base_Device\" + dr("DeviceName") + ".dll") = True Then
                OpenFileDlg.FileName = (System.Environment.CurrentDirectory + "\Base_Device\" + dr("DeviceName") + ".dll")
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

                    AddHandler DirectCast(oObject, Control).MouseDown, AddressOf Me.Propertygridvalues
                    AddHandler DirectCast(oObject, Control).MouseDoubleClick, AddressOf Me.AddDevice
                End With
                ToolTip1.SetToolTip(oObject, oObject.Sname + " Device")
                id = id + 1
            Else
                flag = 1
            End If

        End While
        dr.Close()


        If flag = 1 Then
            MessageBox.Show("Some DLL File are missing from the Source Location")
        End If
        ReDim Preserve devicecount(id)
        sqlconn.Close()
    End Sub

    Public Sub AddDevice(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        dragging = False
        If sender.Sname.ToString <> "Hroad" And sender.Sname.ToString <> "Vroad" Then
            layoutname = InputBox("Enter The Name", "Save As", "Untitled")
        Else
            Dim Str As String = sender.ToString
            substrings = Str.Split("."c)
            layoutname = substrings(0)
        End If

        If String.IsNullOrEmpty(layoutname) Then

        Else
            If String.Compare(layoutname, "") = 0 Or String.Compare(layoutname, "Untitled") = 0 Then
                MessageBox.Show("Enter a Proper name!!!!")
            Else
                Creation(sender)
            End If
        End If

    End Sub
    Public Sub Creation(ByVal sender As Object)

        OpenFileDlg.FileName = (System.Environment.CurrentDirectory + "\Base_Device\" + sender.Sname + ".dll")
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
            .Sname = layoutname + devicecount(sender.Smartid).ToString
            .SLocation = "Panel2"
            .width = 50
            .height = 50
            .Location = New Point(10, pos)
            AddHandler DirectCast(oObject, Control).MouseDown, AddressOf Me.Propertygridvalues
            AddHandler DirectCast(oObject, Control).MouseMove, AddressOf Me.MoveDevice
            AddHandler DirectCast(oObject, Control).MouseUp, AddressOf Me.MoveUp
            AddHandler DirectCast(oObject, Control).MouseClick, AddressOf Me.clickstatus
        End With
        pos = pos + 50
        id1 = id1 + 1

    End Sub
    Public Sub Propertygridvalues(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        PropertyGrid1.SelectedObject = sender
        dragging = True
        beginX = e.X
        beginY = e.Y
        sender.BringToFront()
    End Sub
    Public Sub MoveUp(ByVal sender As Object, ByVal e As MouseEventArgs)
        dragging = False
    End Sub
    Public Sub clickstatus(ByVal sender As Object, ByVal e As MouseEventArgs)
        If sender.ToString <> "" Then
            send = sender
        End If
    End Sub
    Public Sub MoveDevice(ByVal sender As Object, ByVal e As MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Right Then

            For Each ctrl As Control In Panel2.Controls
                If ctrl.ToString <> sender.ToString Then
                    RefreshToolStripMenuItem.Visible = True
                End If
            Next
            If sender.ToString <> "Hroad.hroaddll" And sender.ToString <> "Vroad.vroaddll" Then
                OpenToolStripMenuItem1.Visible = True
                Form3.Label2.Text = sender.Sname.ToString
                envname = sender.Sname.ToString
            Else
                OpenToolStripMenuItem1.Visible = False
            End If
        Else
        If dragging = True Then
            If Panel2.ClientRectangle.Contains(New Rectangle(New Point(sender.Location.X + e.X - beginX, sender.Location.Y + e.Y - beginY), sender.Size)) Then
                sender.Location = New Point(sender.Location.X + e.X - beginX, sender.Location.Y + e.Y - beginY)
            End If
        End If
        End If

    End Sub


    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsToolStripMenuItem.Click
        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.Environment.CurrentDirectory & "\Device.mdb"
        sqlconn.ConnectionString = connString
        sqlquery.Connection = sqlconn
        sqlconn.Open()
        If String.Compare(Label2.Text, "Untitled") = 0 Then
            basename = InputBox("Enter Your Base Name", "Save As", "Untitled")
        Else
            basename = Label2.Text
            TakeScreenShot(Panel2).Save(System.Environment.CurrentDirectory + "\Base_Image\" + basename + ".png", System.Drawing.Imaging.ImageFormat.Png)

        End If
        If String.Compare(basename, "") = 0 Or String.Compare(basename, "Untitled") = 0 Then
            MessageBox.Show("Enter a Proper Base name!!!!")
        Else
            Label2.Text = basename
            Dim id2 As Integer = 1
            Dim o As Object
            Dim str As String
            sqlquery.CommandText = "Delete * from BaseDevice where Environment='" & basename & "'"
            sqlquery.ExecuteNonQuery()
            For Each ctrl As Control In Panel2.Controls
                str = ctrl.ToString
                o = ctrl
                substrings = str.Split("."c)
                sqlquery.CommandText = "INSERT INTO BaseDevice(DeviceNumber,Smartid,Sname,Slocation,Status,DeviceName,DeviceClass,Environment,LocationX,LocationY,Width,Height,Ewidth,Eheight)VALUES('" & id2.ToString & "','" & o.Smartid.ToString & "','" & o.Sname.ToString & "','" & o.Slocation.ToString & "','" & o.Status.ToString & "','" & substrings(0) & "','" & ctrl.ToString & "','" & basename & "','" & ctrl.Location.X & "','" & ctrl.Location.Y & "','" & ctrl.Width & "','" & ctrl.Height & "','" & Panel2.Width & "','" & Panel2.Height & "');"
                sqlquery.ExecuteNonQuery()
                id2 = id2 + 1
            Next
            MessageBox.Show("Device Added Successfully")
            dr.Close()
        End If
        sqlconn.Close()
    End Sub
    Private Function TakeScreenShot(ByVal Control As Control) As Bitmap
        Dim tmpImg As New Bitmap(Control.Width, Control.Height)
        Using g As Graphics = Graphics.FromImage(tmpImg)
            g.CopyFromScreen(Panel2.PointToScreen(New Point(0, 0)), New Point(0, 0), New Size(Panel2.Width, Panel2.Height))
        End Using
        Return tmpImg
    End Function

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        Form5.Show()
    End Sub

    Private Sub CreateDeviceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreateDeviceToolStripMenuItem.Click
        Me.Hide()
        DeviceCreationvb.Show()
    End Sub

    Private Sub RefreshToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshToolStripMenuItem.Click
        sqlconn.Close()
        indicate = 1
        Form5.Button1_Click(e, e)
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Panel2.Width = Convert.ToString(Convert.ToInt32(TextBox1.Text) * 10)
        Panel2.Height = Convert.ToString(Convert.ToInt32(TextBox2.Text) * 10)
    End Sub
End Class
