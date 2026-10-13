Imports Microsoft.VisualBasic.ApplicationServices
Imports System.Windows.Forms

Namespace My

    Partial Friend Class MyApplication

        Private Sub MyApplication_Startup(
            sender As Object,
            e As StartupEventArgs
        ) Handles Me.Startup

            Using login As New LoginForm()

                If login.ShowDialog() <> DialogResult.OK Then

                    e.Cancel = True

                    Return

                End If

            End Using

        End Sub

    End Class

End Namespace