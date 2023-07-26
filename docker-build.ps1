Function Get-WordsFromPascalCase {
    Param([Parameter(Mandatory=$true)] [string]$String)
    $Words = $String -CSplit "(?=[A-Z])"
    $Words = $Words | Where-Object { $_ }
    $Words = $Words | ForEach-Object { $_.ToLower() }
    return $Words
}

Function Convert-ToPascalCase {
    Param([Parameter(Mandatory=$true)] [string]$String)
    $Words = $String -Split "[-_\s]+"
    $Words = $Words | ForEach-Object { $_.Substring(0,1).ToUpper() + $_.Substring(1) }
    $String = $Words -Join ""
    return $String
}

Function Convert-ToCamelCase {
    Param([Parameter(Mandatory=$true, ValueFromPipeline=$true)] [string]$String)
    $Words = Get-WordsFromPascalCase -String $String
    $Words = $Words | ForEach-Object { $_.Substring(0,1).ToUpper() + $_.Substring(1) }
    $String = $Words -Join ""
    $String = $String.Substring(0,1).ToLower() + $String.Substring(1)
    return $String
}

Function Convert-ToKebabCase {
    Param([Parameter(Mandatory=$true)] [string]$String)
    $Words = Get-WordsFromPascalCase -String $String
    $String = $Words -Join "-"
    return $String
}

Function Convert-ToSnakeCase {
    Param([Parameter(Mandatory=$true)] [string]$String)
    $Words = Get-WordsFromPascalCase -String $String
    $String = $Words -Join "_"
    return $String
}

Function Get-ProjectNameFromDirectory {
    Param([Parameter(Mandatory=$false)] [string]$Type = "KebabCase")

    $CurrentDirectory = Get-Location
    while (!(Test-Path ".git")) {
        Set-Location ..
    }

    $ProjectDirectory = Get-Location
    Set-Location $CurrentDirectory

    $ProjectDirectory = $ProjectDirectory -split "\\"
    $ProjectDirectory = $ProjectDirectory[-1]

    $CamelCaseName = Convert-ToPascalCase -String $ProjectDirectory

    if ($Type -eq "CamelCase") {
        $ProjectDirectory = Convert-ToCamelCase -String $ProjectDirectory
    }   

    elseif ($Type -eq "SnakeCase") {
        $ProjectDirectory = Convert-ToSnakeCase -String $ProjectDirectory
    }

    elseif ($Type -eq "KebabCase") {
        $ProjectDirectory = Convert-ToKebabCase -String $ProjectDirectory
    }

    elseif ($Type -eq "PascalCase") {
        $ProjectDirectory = $CamelCaseName
    }

    return $ProjectDirectory
}

Function Get-ProjectNameFromFile {
    $ProjectName = Get-Content -Path "project-name.txt"
    return $ProjectName.Trim()
}

Function Get-ProjectName {
    if (Test-Path "project-name.txt") {
        return Get-ProjectNameFromFile
    }
    else {
        return Get-ProjectNameFromDirectory -Type "KebabCase"
    }
}

Function Get-GitProjectVersion {
    $Command = "git describe --tags --always"
    $stdout = & $Command
    return $stdout
}

Function Get-TextFileVersion {
    $Version = Get-Content -Path "version.txt"
    return $Version.Trim()
}

Function Get-ProjectVersion {
    if (Test-Path "version.txt") {
        return Get-TextFileVersion
    }
    else {
        return Get-GitProjectVersion
    }
}

$Version = Get-ProjectVersion
$ProjectName = Get-ProjectName
$DockerImageName = "$($ProjectName):$($Version)"
$DockerArchiveName = "$($ProjectName)-$($Version)"

Write-Host "Building Docker image: $DockerImageName"

$CurrentDirectory = Get-Location
$OutputTar = "$($CurrentDirectory)\$($DockerArchiveName).tar"

docker build . -t $DockerImageName
docker save "$($DockerImageName)" --output $OutputTar