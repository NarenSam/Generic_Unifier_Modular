Imports System.IO.Directory
Imports System.Data.OleDb
Imports System.Reflection
Public Class DeviceCreationvb
    Public basename As String = ""
    Dim fd As OpenFileDialog = New OpenFileDialog()
    Dim oObject As System.Object
    Dim ThisAssembly As Assembly
    Dim send As Object
    Dim sqlconn As New OleDb.OleDbConnection
    Dim connString As String
    Dim sqlquery As New OleDb.OleDbCommand
    Dim flag As Integer = 0
    Dim dr As OleDb.OleDbDataReader
    Dim OpenFileDlg As New OpenFileDialog
    Dim substrings() As String
    Dim name_class As String
    Dim pos As Integer = 10
    Dim cnt As Integer = 0
    Dim id As Integer = 1

    Private Sub btnclose(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.FormClosing
        Base.Show()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        fd.Title = "Open File Dialog"
        fd.InitialDirectory = "C:\Users\Trinity\Desktop\"
        fd.Filter = "DLL File|*.dll|All|*.*"
        fd.FilterIndex = 1
        fd.RestoreDirectory = True
        If fd.ShowDialog() = DialogResult.OK Then
            TextBox1.Text = fd.FileName
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
            For Each ctrl As Control In Panel1.Controls
                If ctrl.ToString = name_class Then
                    MessageBox.Show("Already Exist")
                    flag = 1
                End If
            Next
            If flag = 0 Then
                TypeObj = ThisAssembly.GetType(name_class)
                oObject = Activator.CreateInstance(TypeObj)
                Panel1.Controls.Add(oObject)
                oObject.width = 50
                oObject.height = 50
                oObject.Location = New Point(oObject.Location.X + 15, oObject.Location.Y + pos)
                With Panel1.Controls
                    AddHandler DirectCast(oObject, Control).MouseClick, AddressOf Me.clickstat
                    AddHandler DirectCast(oObject, Control).MouseClick, AddressOf Me.Propertygridvalues
                End With

                pos = pos + 60
                If System.IO.File.Exists(System.Environment.CurrentDirectory + "\Base_Device\" + System.IO.Path.GetFileName(fd.FileName)) Then
                    System.IO.File.Delete(System.Environment.CurrentDirectory + "\Base_Device\" + System.IO.Path.GetFileName(fd.FileName))
                    My.Computer.FileSystem.CopyFile(fd.FileName, System.Environment.CurrentDirectory + "\Base_Device\" + System.IO.Path.GetFileName(fd.FileName))

                Else
                    My.Computer.FileSystem.CopyFile(fd.FileName, System.Environment.CurrentDirectory + "\Base_Device\" + System.IO.Path.GetFileName(fd.FileName))
                End If

            End If

            flag = 0
        End If

    End Sub
    Public Sub clickstat(ByVal sender As Object, ByVal e As MouseEventArgs)
        send = sender
    End Sub
    Public Sub Propertygridvalues(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        PropertyGrid1.SelectedObject = sender
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim s As String = ""
        sqlquery.CommandText = "Select * From DeviceNS where deviceclass='" & send.ToString & "'"
        dr = sqlquery.ExecuteReader
        dr.Read()
        s = dr("DeviceName")
        dr.Close()
        sqlquery.CommandText = "delete * from DeviceNS where deviceclass='" & send.ToString & "'"
        sqlquery.ExecuteNonQuery()
        sqlquery.CommandText = "delete * from TotalDevice where devicelist='" & s & "'"
        sqlquery.ExecuteNonQuery()
        MessageBox.Show("Successfully Deleted")
        DeviceArrange()
        Me.Controls.Clear()
        InitializeComponent()
        sqlconn.Close()

        DeviceCreationvb_Load(e, e)

        dr.Close()
    End Sub
    Private Sub DeviceArrange()
        pos = 10
        Dim DeviceList() As String = {""}
        Dim t1 As Integer = 0
        sqlquery.CommandText = "select * from DeviceNS"
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
                sqlquery.CommandText = "Update DeviceNS set DeviceNumber='" & id.ToString & "', LocationX='15', LocationY='" & pos.ToString & "' where DeviceName='" & DeviceList(i) & "'"
                sqlquery.ExecuteNonQuery()
                pos += 60
            End If
        Next
        pos = 10
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim control As String = New String("")
        For Each ctrl As Control In Panel1.Controls
            control = control + ctrl.ToString + vbLf
        Next
        MessageBox.Show(control)
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        Dim str As String
        sqlquery.CommandText = "delete * from DeviceNS"
        sqlquery.ExecuteNonQuery()
        For Each ctrl As Control In Panel1.Controls
            str = ctrl.ToString
            substrings = str.Split("."c)
            sqlquery.CommandText = "INSERT INTO DeviceNS(DeviceNumber,DeviceName,DeviceClass,LocationX,LocationY,Width,Height)VALUES('" & id.ToString & "','" & substrings(0) & "','" & ctrl.ToString & "','" & ctrl.Location.X & "','" & ctrl.Location.Y & "','" & ctrl.Width & "','" & ctrl.Height & "');"
            sqlquery.ExecuteNonQuery()
            sqlquery.CommandText = "INSERT INTO TotalDevice(DeviceID,DeviceList)VALUES('" & id.ToString & "','" & substrings(0) & "');"
            sqlquery.ExecuteNonQuery()
            id = id + 1
        Next
        MessageBox.Show("Device Saved Successfully")
        sqlconn.Close()
    End Sub

    Private Sub DeviceCreationvb_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.Environment.CurrentDirectory & "\Device.mdb"
        sqlconn.ConnectionString = connString
        sqlquery.Connection = sqlconn
        sqlconn.Open()
        sqlquery.CommandText = "select * from DeviceNS"
        dr = sqlquery.ExecuteReader

        While dr.Read

            pos = dr("LocationY")
           
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
                cnt = 0
                TypeObj = ThisAssembly.GetType(name_class)
                oObject = Activator.CreateInstance(TypeObj)
                Panel1.Controls.Add(oObject)
                With Me.Panel1.Controls
                    AddHandler DirectCast(oObject, Control).MouseClick, AddressOf Me.clickstat
                    AddHandler DirectCast(oObject, Control).MouseClick, AddressOf Me.Propertygridvalues
                End With
                oObject.width = dr("Width")
                oObject.height = dr("Height")
                oObject.Location = New Point(dr("LocationX"), pos)
            Else
                flag = 1
            End If
            pos = pos + 60
        End While
        If flag = 1 Then
            MessageBox.Show("Some DLL File are missing from the Source Location")
            Me.Close()
        End If
        dr.Close()

    End Sub
End Class