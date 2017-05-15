Public Class ScenarioInput
    Public sqlconn As New OleDb.OleDbConnection
    Public connString As String
    Public sqlquery As New OleDb.OleDbCommand
    Public dr As OleDb.OleDbDataReader
    Dim flag3 As Integer = 0
    Public valid As Integer = 0
    Public scenario_no As Integer = 1
    Public Allow As Integer = 0
    Private Sub ScenarioInput_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label2.Text = "Scenario_" + scenario_no.ToString
        TextBox2.Visible = False
        ComboBox4.Visible = False
        scenario_no += 1
        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & System.Environment.CurrentDirectory & "\Device.mdb"
        sqlconn.ConnectionString = connString
        sqlquery.Connection = sqlconn
        sqlconn.Open()
        sqlquery.CommandText = "select * from EnvironmentDevice where BaseName ='" & input.TextBox3.Text & "' and Environment ='" & Form3.Label2.Text & "'"
        dr = sqlquery.ExecuteReader
        While dr.Read
            If dr("DeviceName") = "user" Then
                ComboBox1.Items.Add(dr("Sname"))
            ElseIf dr("DeviceName") = "Room" Then
                ComboBox2.Items.Add(dr("Sname"))
            End If
        End While
        ComboBox3.Items.Add("Morning")
        ComboBox3.Items.Add("Afternoon")
        ComboBox3.Items.Add("Evening")
        ComboBox3.Items.Add("Night")
        ComboBox3.Items.Add("Any Time")
        ComboBox3.Items.Add("Specify The Time")

        ComboBox4.Items.Add("AM")
        ComboBox4.Items.Add("PM")
        dr.Close()
    End Sub
  
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Allow = 1 Then
            Dim hours As Integer = DateTime.Now.Hour
            Dim daypart As String = ""
            Dim TimeValue As Double = Convert.ToDouble(TextBox2.Text)
            TimeValue = Math.Round(TimeValue, 2)
            If ComboBox4.SelectedItem = "AM" Then
                hours = TimeValue
            ElseIf ComboBox4.SelectedItem = "PM" Then
                hours = 12 + TimeValue
            End If
            If hours < 12 Then
                ComboBox3.Text = "Morning"
            ElseIf hours <= 17 Then
                ComboBox3.Text = "Afternoon"
            ElseIf hours <= 19 Then
                ComboBox3.Text = "Evening"
            ElseIf hours <= 23 Then
                ComboBox3.Text = "Night"
            End If
        End If
        Dim flag As Integer = 0
        Dim flag1 As Integer = 0
        Dim OnDevice As String = ""
        Dim OfDevice As String = ""
        If flag3 = 0 Then
            sqlquery.CommandText = "Delete * from UserAction"
            sqlquery.ExecuteNonQuery()
        End If
        sqlquery.CommandText = "select * from ActionWord"
        dr = sqlquery.ExecuteReader
        While dr.Read
            If TextBox1.Text = dr("Action") Then
                valid = 1
            End If
        End While
        dr.Close()
        If valid <> 1 Then
            MessageBox.Show("Please Enter The valid Action")
        Else
            Dim device1() As String = RichTextBox1.Lines.Clone
            Dim device2() As String = input.RichTextBox2.Lines.Clone
            Dim device3() As String = input.RichTextBox3.Lines.Clone
            For i As Integer = 0 To device1.Length - 1
                For j As Integer = 0 To device2.Length - 1
                    If device1(i) = device2(j) Then
                        flag = 1
                    End If
                Next
                For k As Integer = 0 To device3.Length - 1
                    If device1(i) = device3(k) Then
                        flag1 = 1
                    End If
                Next
                If flag = 0 And flag1 = 0 Then
                    MessageBox.Show(device1(i) + " Not Present in " + input.TextBox2.Text)
                End If
                If flag1 = 1 Then
                    MessageBox.Show(device1(i) + " is not the Device")
                End If
                flag = flag1 = 0
            Next
            For i As Integer = 0 To device1.Length - 1
                OnDevice = OnDevice + device1(i) + ","
            Next
            OnDevice = OnDevice.Trim(",")
            device2 = RichTextBox2.Lines.Clone
            For i As Integer = 0 To device2.Length - 1
                OfDevice = OfDevice + device2(i) + ","
            Next
            OfDevice = OfDevice.Trim(",")
            sqlquery.CommandText = "INSERT INTO UserAction(ScenarioNo,BaseName,Environment,UserName,UserAction,Location,Timing,TurnON_Devices,TurnOFF_Devices)VALUES('" & Label2.Text & "','" & input.TextBox3.Text & "','" & input.TextBox2.Text & "','" & ComboBox1.Text & "','" & TextBox1.Text.ToLower & "','" & ComboBox2.Text & "','" & ComboBox3.Text & "','" & OnDevice & "','" & OfDevice & "');"
            sqlquery.ExecuteNonQuery()
            MessageBox.Show("Successfully Updated")
            Dim response As String
            response = MsgBox("Are You Want To Add Scenario ?", vbYesNo)
            If response = vbYes Then
                Label2.Text = "Scenario_" + scenario_no.ToString
                scenario_no += 1
                ComboBox1.Text = ""
                ComboBox2.Text = ""
                ComboBox3.Text = ""
                TextBox1.Text = ""
                RichTextBox1.Text = ""
                RichTextBox2.Text = ""
                flag3 = 1
                Allow = 0
            Else
                Me.Hide()
                input.Button1.Enabled = True
                flag3 = 0
                Allow = 0
            End If
        End If
    End Sub

    Private Sub ComboBox3_SelectionChangeCommited(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectionChangeCommitted

        If ComboBox3.SelectedItem = "Specify The Time" Then
            TextBox2.Visible = True
            ComboBox4.Visible = True
            ComboBox3.Visible = False
            Allow = 1
        End If
        
    End Sub
End Class