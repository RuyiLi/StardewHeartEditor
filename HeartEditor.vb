Public Class HeartEditor

    Dim save As String
    Dim savePath As String

    Private Sub browse_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        Dim fd As OpenFileDialog = New OpenFileDialog()

        fd.Title = "Open Save File"
        fd.Filter = "All files (*.*)|*.*|All files (*.*)|*.*"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then
            savePath = fd.FileName
            save = My.Computer.FileSystem.ReadAllText(savePath)
            save = Replace(save, Chr(13), "")
            Do While InStr(1, save, "  ")
                save = Replace(save, "  ", "")
            Loop
            Dim xml As XElement = xml.Parse(save)

            Dim friends As IEnumerable(Of XElement) = xml.Descendants("friendships").Descendants("item")
            villagers.Items.Clear()
            For Each f As XElement In friends
                Dim person As String
                person = f.Descendants("key").Descendants("string").Value
                villagers.Items.Add(person)
            Next
        End If

    End Sub

    Private Sub villagers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles villagers.SelectedIndexChanged
        Dim current As String = villagers.SelectedItem.ToString()

        Dim xml As XElement = xml.Parse(save)

        Dim friends As IEnumerable(Of XElement) = xml.Descendants("friendships").Descendants("item")

        For Each f As XElement In friends
            Dim person As String
            person = f.Descendants("key").Descendants("string").Value
            If (String.Equals(person, current)) Then
                Dim points As Integer
                Dim hearts As Integer
                points = f.Descendants("value").Descendants("ArrayOfInt").Elements("int").Value
                hearts = Integer.Parse(f.Descendants("value").Descendants("ArrayOfInt").Elements("int").Value) / 250
                friendshipPoints.Text = points
                numHearts.Text = hearts.ToString()
                villagerName.Text = person
            End If
        Next

    End Sub

    Private Sub update_Click(sender As Object, e As EventArgs) Handles update.Click
        Dim xml As XElement = xml.Parse(save)
        Dim friends As IEnumerable(Of XElement) = xml.Descendants("friendships").Descendants("item")
        Dim current As String = villagers.SelectedItem.ToString()
        For Each f As XElement In friends
            Dim person As String
            person = f.Descendants("key").Descendants("string").Value
            If (String.Equals(person, current)) Then
                Dim p As String = f.Descendants("value").Descendants("ArrayOfInt").Elements("int")(0).Value
                log(person + ": " + p + " -> " + friendshipPoints.Text)

                save = save.Replace("<key><string>" + current + "</string></key><value><ArrayOfInt><int>" + f.Descendants("value").Descendants("ArrayOfInt").Elements("int")(0).Value + "</int>", "<key><string>" + current + "</string></key><value><ArrayOfInt><int>" + friendshipPoints.Text + "</int>")
            End If
        Next
    End Sub

    Private Sub numHearts_TextChanged(sender As Object, e As EventArgs) Handles numHearts.TextChanged
        If (Integer.TryParse(numHearts.Text, Nothing)) Then
            Dim numberOfHearts As Integer
            numberOfHearts = Integer.Parse(numHearts.Text) * 250
            friendshipPoints.Text = numberOfHearts.ToString()
        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        Dim file As System.IO.StreamWriter
        file = My.Computer.FileSystem.OpenTextFileWriter(savePath, False)
        file.Write(save)
        MsgBox("File successfully saved at " + savePath, vbOKOnly, "Save")
        file.Close()
    End Sub

    Private Sub log(msg As String)
        System.Console.WriteLine(msg)
    End Sub

    Private Sub FriendshipPointsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FriendshipPointsToolStripMenuItem.Click
        MsgBox("Friendship Points are the numbers that determine how close you are to a villager. Each heart Is 250 friendship points, so 2500 friendship points would mean 10 hearts.", vbOKOnly, "Help")
    End Sub

    Private Sub WhyArentSomeVillagersAppearingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WhyArentSomeVillagersAppearingToolStripMenuItem.Click
        MsgBox("You may find that some villagers may be missing from the list. For a villager to show up on the list, you must interact with them at least once in some way ingame.", vbOKOnly, "Help")
    End Sub

    Private Sub WhatDoIDoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WhatDoIDoToolStripMenuItem.Click
        MsgBox("This tool changes your relationship with a villager. You can automatically max their hearts out without having to do it manually.", vbOKOnly, "Help")
    End Sub

    Private Sub WhereIsMySaveFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WhereIsMySaveFileToolStripMenuItem.Click
        MsgBox("For Windows, your save file should be located at C:\Users\[username]\AppData\Roaming\StardewValley\Saves\[characterName_randomNumbers]\[characterName_randomNumbers]", vbOKOnly, "Help")
    End Sub

    Private Sub WhyArentTheValuesUpdatingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WhyArentTheValuesUpdatingToolStripMenuItem.Click
        MsgBox("To save your changes on a villager, click update.", vbOKOnly, "Help")
    End Sub
End Class
