Imports System.IO.Directory
Imports System.Data.OleDb
Imports System.Reflection
Imports System
Imports System.IO
Public Class DeviceCreation1
    Public sqlconn As New OleDb.OleDbConnection
    Public connString As String
    Public sqlquery As New OleDb.OleDbCommand
    Dim str As String

    Dim flag As Integer = 0
    Public dr As OleDb.OleDbDataReader
    Public OpenFileDlg As New OpenFileDialog

    Dim fd As OpenFileDialog = New OpenFileDialog()
    Dim oObject As System.Object
    Dim ThisAssembly As Assembly
    Dim send As Object
    Dim basename As String
    Dim substrings() As String
    Dim name_class As String
    Dim ypos As Integer = 44
    Dim xpos As Integer = 3
    Dim cnt As Integer = 0
    Dim t1 As Integer = 0
    Dim id As Integer = 1
    Private Sub btnclose(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.FormClosing
        Form3.sqlconn.Close()
        Form3.Show()
        Form3.Label2.Text = basename
        Base.OpenToolStripMenuItem1_Click(e, e)
    End Sub
    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        fd.Title = "Open File Dialog"
        fd.InitialDirectory = "C:\Users\Trinity\Desktop\"
        fd.Filter = "DLL File|*.dll|All|*.*"
        fd.FilterIndex = 1
        fd.RestoreDirectory = True
        If fd.ShowDialog() = DialogResult.OK Then
            TextBox2.Text = fd.FileName
            ThisAssembly = Assembly.LoadFrom(fd.FileName)
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
            For Each ctrl As Control In Me.Panel1.Controls
                If ctrl.ToString = name_class Then
                    MessageBox.Show("Already Exist")
                    flag = 1
                End If
            Next
            If flag = 0 Then
                TypeObj = ThisAssembly.GetType(name_class)
                oObject = Activator.CreateInstance(TypeObj)
                Panel1.Controls.Add(oObject)
                oObject.width = 35
                oObject.height = 35
                oObject.Location = New Point(oObject.Location.X + xpos, oObject.Location.Y + ypos)
                oObject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                With Me.Panel1.Controls
                    AddHandler DirectCast(oObject, Control).MouseClick, AddressOf Me.Clickstatus
                    AddHandler DirectCast(oObject, Control).MouseClick, AddressOf Me.Propertygridvalues
                End With
                xpos = xpos + 41
                t1 = t1 + 1
                If t1 = 2 Then
                    t1 = 0
                    xpos = 3
                    ypos = ypos + 41
                End If
                If System.IO.File.Exists(System.Environment.CurrentDirectory + "\Environment_Device\" + System.IO.Path.GetFileName(fd.FileName)) Then
                    System.IO.File.Delete(System.Environment.CurrentDirectory + "\Environment_Device\" + System.IO.Path.GetFileName(fd.FileName))
                    My.Computer.FileSystem.CopyFile(fd.FileName, System.Environment.CurrentDirectory + "\Environment_Device\" + System.IO.Path.GetFileName(fd.FileName))
                Else
                    My.Computer.FileSystem.CopyFile(fd.FileName, System.Environment.CurrentDirectory + "\Environment_Device\" + System.IO.Path.GetFileName(fd.FileName))
                End If
                flag = 0
            End If
        End If
    End Sub

    Public Sub Propertygridvalues(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        PropertyGrid1.SelectedObject = sender
    End Sub

    Public Sub Clickstatus(ByVal sender As Object, ByVal e As MouseEventArgs)
        send = sender
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        Dim s As String = ""

        If send.sname <> "user" Then
            sqlquery.CommandText = "select * from devicens_1 where deviceclass='" & send.ToString & "'"
            dr = sqlquery.ExecuteReader
            dr.Read()
            s = dr("devicename")
            dr.Close()
            sqlquery.CommandText = "delete * from DeviceNS_1 where deviceclass='" & send.ToString & "'"
            sqlquery.ExecuteNonQuery()
            sqlquery.CommandText = "delete * from TotalDevice_1 where devicelist='" & s & "'"
            sqlquery.ExecuteNonQuery()
            MessageBox.Show("Successfully Deleted")
            DeviceArrange()

            Me.Controls.Clear()
            InitializeComponent()
            sqlconn.Close()
            DeviceCreationvb_Load(e, e)
        Else
            MessageBox.Show("Sorry You Can't Delete This User")
        End If
       
    End Sub


    Private Sub DeviceArrange()
        Dim id As Integer = 2
        Dim x As Integer = 3
        Dim y As Integer = 44
        Dim DeviceList() As String = {""}
        t1 = 0
        sqlquery.CommandText = "select * from DeviceNS_1"
        dr = sqlquery.ExecuteReader
        While dr.Read
            ReDim Preserve DeviceList(t1)
            DeviceList(t1) = dr("DeviceName")
            t1 += 1
        End While
        dr.Close()
        t1 = 0
        For i As Integer = 0 To DeviceList.Length - 1
            If DeviceList(i) <> "Room" Then
                sqlquery.CommandText = "Update DeviceNS_1 set DeviceNumber='" & id.ToString & "', LocationX='" & x.ToString & "', LocationY='" & y.ToString & "' where DeviceName='" & DeviceList(i) & "'"
                sqlquery.ExecuteNonQuery()
                x = x + 41
                t1 = t1 + 1
                If t1 = 2 Then
                    t1 = 0
                    x = 3
                    y = y + 41
                End If
                id += 1
            End If
        Next

    End Sub



    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim control As String = New String("")
        Dim count As Integer = 1
        control = "Total Device Present" + vbLf
        For Each ctrl As Control In Panel1.Controls
            control = control + count.ToString + ".)" + ctrl.ToString + vbLf
            count += 1
        Next
        MessageBox.Show(control)
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        sqlquery.CommandText = "Delete * from TotalDevice_1"
        sqlquery.ExecuteNonQuery()
        sqlquery.CommandText = "Delete * from DeviceNS_1"
        sqlquery.ExecuteNonQuery()
        For Each ctrl As Control In Panel1.Controls
            Str = ctrl.ToString
            substrings = Str.Split("."c)
            sqlquery.CommandText = "INSERT INTO DeviceNS_1(DeviceNumber,DeviceName,DeviceClass,LocationX,LocationY,Width,Height)VALUES('" & id.ToString & "','" & substrings(0) & "','" & ctrl.ToString & "','" & ctrl.Location.X & "','" & ctrl.Location.Y & "','" & ctrl.Width & "','" & ctrl.Height & "');"
            sqlquery.ExecuteNonQuery()

            sqlquery.CommandText = "INSERT INTO TotalDevice_1(DeviceID,DeviceList,Overall_User)VALUES('" & id.ToString & "','" & substrings(0) & "','0');"
            sqlquery.ExecuteNonQuery()
            id = id + 1
        Next
        MessageBox.Show("Device Saved Successfully")
    End Sub

    Private Sub DeviceCreationvb_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        basename = Form3.Label2.Text

        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.Environment.CurrentDirectory & "\Device.mdb"
        sqlconn.ConnectionString = connString
        sqlquery.Connection = sqlconn
        sqlconn.Open()
        sqlquery.CommandText = "select * from devicens_1"
        dr = sqlquery.ExecuteReader

        While dr.Read
            xpos = dr("locationx")
            ypos = dr("locationy")

            If System.IO.File.Exists(System.Environment.CurrentDirectory + "\Environment_Device\" + dr("devicename") + ".dll") = True And dr("devicename") <> "room" Then
                OpenFileDlg.FileName = (System.Environment.CurrentDirectory + "\Environment_Device\" + dr("devicename") + ".dll")
                ThisAssembly = Assembly.LoadFrom(OpenFileDlg.FileName)
                Dim typeobj As Type
                For Each typeobj In ThisAssembly.GetTypes()
                    Dim t As Integer = 0
                    Dim iteration As Integer = 0
                    For Each c As Char In typeobj.ToString
                        If c = "." Then
                            t = t + 1
                        End If
                    Next
                    If t = 1 Then
                        name_class = typeobj.ToString
                        Exit For
                    Else
                        t = 0
                    End If
                Next
                cnt = 0
                typeobj = ThisAssembly.GetType(name_class)
                oObject = Activator.CreateInstance(typeobj)
                Panel1.Controls.Add(oObject)
                With Me.Panel1.Controls
                    AddHandler DirectCast(oObject, Control).MouseClick, AddressOf Me.Clickstatus
                    AddHandler DirectCast(oObject, Control).MouseClick, AddressOf Me.Propertygridvalues
                End With
                oObject.sname = dr("devicename")
                oObject.width = dr("width")
                oObject.height = dr("height")
                oObject.location = New Point(xpos, ypos)

            End If
            If xpos = 3 And ypos = 3 Then
                xpos = 3
                ypos = 44
            Else
                If xpos = 3 Then
                    xpos += 41
                Else
                    xpos = 3
                    ypos += 41
                End If
            End If
        End While
        dr.Close()
        
    End Sub

End Class