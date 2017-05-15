Public Class ChooseChart

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If RadioButton1.Checked = True Then
            SpecUser.Show()
            Me.Hide()
        ElseIf RadioButton2.Checked = True Then
            FreqDevice.Show()
            Me.Hide()
        End If
    End Sub

    Private Sub btnclose(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.FormClosing
        Form1.Show()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
        Form1.Show()
    End Sub

    Private Sub ChooseChart_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = ""
    End Sub
End Class