Imports System.Data.OleDb
Imports System.IO.Directory
Imports System.Reflection
Public Class Form5
    Public conn As OleDbConnection
    Public cmd As OleDbCommand
    Public da As OleDb.OleDbDataAdapter
    Public ds As DataSet
    Public dr As OleDb.OleDbDataReader
    Public itemcoll(100) As String
    Public oObject As System.Object
    Public cnt As Integer = 0
    Public name_class As String
    Public ThisAssembly As Assembly
    Public OpenFileDlg As New OpenFileDialog
    Public strQ As String

    Public Sub Form5_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        
        Me.ListView1.View = View.Details
        Me.ListView1.GridLines = True
        conn = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" &
            System.Environment.CurrentDirectory & "\Device.mdb")
        strQ = "SELECT DISTINCT Environment FROM BaseDevice"
        cmd = New OleDbCommand(strQ, conn)
        conn.Open()
        da = New OleDbDataAdapter(cmd)
        ds = New DataSet
        da.Fill(ds, "BaseDevice")
        Dim i As Integer = 0
        Dim j As Integer = 0
        ' adding the columns in ListView
        For i = 0 To ds.Tables(0).Columns.Count - 1
            Me.ListView1.Columns.Add("Environment")
        Next

        'Now adding the Items in Listview
        For i = 0 To ds.Tables(0).Rows.Count - 1
            For j = 0 To ds.Tables(0).Columns.Count - 1
                itemcoll(j) = ds.Tables(0).Rows(i)(j).ToString()
            Next
            Dim lvi As New ListViewItem(itemcoll)
            Me.ListView1.Items.Add(lvi)
        Next
        ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
    End Sub

    Public Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Base.Close()
        Form3.Close()
        Dim bs As String
        bs = ListView1.SelectedItems(0).Text()
        Base.Label2.Text = bs

        cmd.CommandText = "SELECT * FROM BaseDevice where Environment='" & bs & "'"

        dr = cmd.ExecuteReader
        While dr.Read
            OpenFileDlg.FileName = (System.Environment.CurrentDirectory + "\" + dr("DeviceName") + ".dll")
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
            Base.Panel2.Width = dr("Ewidth")
            Base.Panel2.Height = dr("Eheight")

            Base.Panel2.Controls.Add(oObject)
            With oObject
                .Smartid = dr("Smartid")
                .Sname = dr("Sname")
                .SLocation = dr("Slocation")
                .width = dr("Width")
                .height = dr("Height")
                .Location = New Point(dr("LocationX"), dr("LocationY"))
                AddHandler DirectCast(oObject, Control).MouseDown, AddressOf Base.Propertygridvalues
                AddHandler DirectCast(oObject, Control).MouseMove, AddressOf Base.MoveDevice
                AddHandler DirectCast(oObject, Control).MouseUp, AddressOf Base.MoveUp
                AddHandler DirectCast(oObject, Control).MouseClick, AddressOf Base.clickstatus
            End With
            Base.id1 = Base.id1 + 1
        End While
        dr.Close()
        Me.Hide()
        Base.Show()
    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If Base.Visible = True Then
            Me.Close()

        Else
            Me.Close()
            Form2.Show()
        End If

    End Sub

    Private Sub btnclose(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.FormClosing
        Form2.Show()
    End Sub

End Class