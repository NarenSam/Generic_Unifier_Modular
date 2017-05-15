Imports System.Data.OleDb
Imports System.Reflection
Public Class Form7
    Dim oObject As System.Object
    Dim ThisAssembly As Assembly
    Dim name_class As String
    Public OpenFileDlg As New OpenFileDialog
    Public sqlconn As New OleDb.OleDbConnection
    Public connString As String
    Public sqlquery As New OleDb.OleDbCommand
    Public dr As OleDb.OleDbDataReader
    Public id As Integer = 0
    Private Sub Form7_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim toolTip1 As New ToolTip()
        toolTip1.ShowAlways = True
        toolTip1.SetToolTip(Me.RichTextBox2, "Add(Device_Name,x_loc,y_loc)" & vbNewLine & "Set(Device_Name,x_loc,y_loc)" & vbNewLine & "Delete(Device_Name)")

        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.Environment.CurrentDirectory & "\Device.mdb"
        sqlconn.ConnectionString = connString
        sqlquery.Connection = sqlconn
        sqlconn.Open()
        RichTextBox1.AppendText("===================================================================================" & vbNewLine)
        RichTextBox1.AppendText("Build ( Environment_Name = " & Form3.Label2.Text & ", Panel_Width = " & Form3.Panel2.Width & ", Panel_Height = " & Form3.Panel2.Height & " )" & vbNewLine)
        RichTextBox1.AppendText("===================================================================================" & vbNewLine)
        RichTextBox1.AppendText("--------------------------------------------------------------------------------------------------------------" & vbNewLine)
        RichTextBox1.AppendText("DEVICE PRESENT IN PANEL" & vbNewLine)
        RichTextBox1.AppendText("--------------------------------------------------------------------------------------------------------------" & vbNewLine)
        For Each ctrl As Control In Form3.Panel2.Controls
            id += 1
            sqlquery.CommandText = "select * from DeviceNS_1"
            dr = sqlquery.ExecuteReader
            While dr.Read
                If ctrl.ToString = dr("DeviceClass") Then
                    RichTextBox1.AppendText("Device(" & UCase(dr("DeviceName")) & ")" & vbNewLine)
                End If
            End While
            RichTextBox1.AppendText("Device_Name(" & CType(ctrl, Object).Sname & ")" & vbNewLine)
            RichTextBox1.AppendText("Device_Location(" & CType(ctrl, Object).Location.X & "," & CType(ctrl, Object).Location.X & ")" & vbNewLine)
            RichTextBox1.AppendText("Device_Location(" & CType(ctrl, Object).Width & "," & CType(ctrl, Object).Height & ")" & vbNewLine)
            RichTextBox1.AppendText("......................................................................................................" & vbNewLine)
            dr.Close()
        Next
        RichTextBox1.AppendText("--------------------------------------------------------------------------------------------------------------" & vbNewLine)
        RichTextBox1.AppendText("===================================================================================" & vbNewLine)
    End Sub
    Private Sub btnclose(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.FormClosing
        Me.Hide()
        Form3.Show()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim AddDevice() As String
        Dim device As String = ""
        Dim DeviceDetail As String = ""
        Dim s() As String = {""}
        Dim exceeds As Integer = 0
        Dim Operation As String = ""

        Dim DeviceSname As String = ""
        Dim DeviceName As String = ""
        Dim DeviceXLocation As Integer = 0
        Dim DeviceYLocation As Integer = 0
        RichTextBox3.Text = ""
        If RichTextBox2.Text <> "" Then
            AddDevice = RichTextBox2.Lines.Clone
            For i As Integer = 0 To AddDevice.Length - 1

                device = AddDevice(i)
                If UCase(device) = "SAVE" Then
                    Form3.SaveAsToolStripMenuItem_Click(e, e)
                End If
                For j As Integer = 0 To device.Length - 1
                    
                    If device(j) <> "(" Then
                        If exceeds = 0 Then
                            Operation = Operation + device(j)
                            If Operation.Length > 7 Then
                                RichTextBox3.AppendText(RichTextBox2.Text + " Syntax Error" & vbNewLine)
                                Exit For
                            End If
                        End If
                    Else
                        exceeds = 1
                        If UCase(Operation) <> "ADD" And UCase(Operation) <> "DELETE" And UCase(Operation) <> "SET" And UCase(Operation) <> "SAVE" Then
                            RichTextBox3.AppendText("Invalid Operation " + Operation & vbNewLine)
                            Exit For
                        End If

                        If device(device.Length - 1) <> ")" Then
                            RichTextBox3.AppendText("Syntax Error " & vbNewLine)
                            Exit For
                        End If

                        If UCase(Operation) = "ADD" Then
                            For k As Integer = j + 1 To device.Length - 2
                                DeviceDetail = DeviceDetail + device(k)
                            Next
                            s = DeviceDetail.Split(","c)
                            If s.Length <> 3 Then
                                RichTextBox3.AppendText(DeviceDetail + " Missing Statement" & vbNewLine)
                                Exit For
                            End If
                            DeviceName = s(0)
                            DeviceXLocation = Convert.ToInt32(s(1))
                            DeviceYLocation = Convert.ToInt32(s(2))

                            If DeviceXLocation > Form3.Panel2.Width And DeviceYLocation > Form3.Panel2.Height Then
                                RichTextBox3.AppendText(DeviceDetail + " Location Error" & vbNewLine)
                                Exit For
                            End If
                            If DeviceXLocation < 0 And DeviceYLocation < 0 Then
                                RichTextBox3.AppendText(DeviceDetail + " Location Error" & vbNewLine)
                                Exit For
                            End If
                            Dim present As Integer = 0
                            sqlquery.CommandText = "select * from TotalDevice_1"
                            dr = sqlquery.ExecuteReader
                            While dr.Read
                                If UCase(DeviceName) = UCase(dr("DeviceList")) Then
                                    present = 1
                                End If
                            End While
                            dr.Close()
                            If present = 0 Then
                                RichTextBox3.AppendText(DeviceName + " Unknown Device" & vbNewLine)
                                Exit For
                            End If
                            Dim n As Integer = 1
                            For Each ctrl As Control In Form3.Panel2.Controls
                                If String.Compare(CType(ctrl, Object).Sname, DeviceName) = 0 Then
                                    n = n + 1
                                End If
                            Next
                            n += 1
                            If System.IO.File.Exists(System.Environment.CurrentDirectory + "\Environment_Device\" + DeviceName + ".dll") = True And DeviceName <> "Room" Then
                                OpenFileDlg.FileName = (System.Environment.CurrentDirectory + "\Environment_Device\" + DeviceName + ".dll")
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

                                id += 1
                                Form3.Panel2.Controls.Add(oObject)
                                With oObject
                                    .Smartid = id
                                    .Sname = DeviceName + n.ToString
                                    .SLocation = Form3.Label2.Text
                                    .width = 47
                                    .height = 47
                                    .Location = New Point(DeviceXLocation, DeviceYLocation)
                                    AddHandler DirectCast(oObject, Control).MouseDown, AddressOf Form3.Propertygridvalues
                                    AddHandler DirectCast(oObject, Control).MouseMove, AddressOf Form3.MoveDevice
                                    AddHandler DirectCast(oObject, Control).MouseUp, AddressOf Form3.MoveUp
                                    AddHandler DirectCast(oObject, Control).MouseClick, AddressOf Form3.clickstatus
                                End With

                            End If
                            RichTextBox3.AppendText(RichTextBox2.Text + "-> Device " + DeviceName + " Successfully Added" & vbNewLine)

                            Exit For
                            n = 0
                        End If
                        If UCase(Operation) = "DELETE" Then
                            For k As Integer = j + 1 To device.Length - 2
                                DeviceDetail = DeviceDetail + device(k)
                            Next
                            For Each ctrl As Control In Form3.Panel2.Controls
                                If CType(ctrl, Object).Sname = DeviceDetail Then
                                    CType(ctrl, Object).dispose()
                                    CType(ctrl, Object).Refresh()
                                    Form3.id1 -= 1
                                    RichTextBox3.AppendText(AddDevice(i) + "->" + DeviceName + " Device Successfully Deleted" & vbNewLine)
                                End If
                            Next
                        End If
                        If UCase(Operation) = "SET" Then
                            For k As Integer = j + 1 To device.Length - 2
                                DeviceDetail = DeviceDetail + device(k)
                            Next
                            s = DeviceDetail.Split(","c)

                            If s.Length > 4 Then
                                RichTextBox3.AppendText(DeviceDetail + " Missing Statement Here" & vbNewLine)
                                Exit For
                            End If

                            DeviceSname = s(0)
                            DeviceXLocation = s(1)
                            DeviceYLocation = s(2)

                            For Each ctrl As Control In Form3.Panel2.Controls
                                If CType(ctrl, Object).Sname = DeviceSname Then
                                    CType(ctrl, Object).Location = New Point(DeviceXLocation, DeviceYLocation)
                                    RichTextBox3.AppendText(AddDevice(i) + "->" + DeviceName + " Device Successfully Moved" & vbNewLine)
                                End If
                            Next
                        End If
                        End If
                Next
                Operation = ""
                DeviceSname = ""
                DeviceName = ""
                DeviceXLocation = 0
                DeviceYLocation = 0
                DeviceDetail = ""
                exceeds = 0
            Next
        Else
            MessageBox.Show("Add Code in Work Area")
        End If
        sqlconn.Close()
        RichTextBox1.Text = ""
        Form7_Load(e, e)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        Me.Close()

    End Sub
End Class