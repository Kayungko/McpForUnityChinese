#Requires -Version 5.1
<#
.SYNOPSIS
    打包 McpForUnityChinese 补丁并发布 GitHub Release。

.PARAMETER Version
    发布版本号，形如 v1.0.0。会同时作为 git tag 名称和 Release 标题。

.PARAMETER Force
    跳过工作区未提交更改的校验，以及推送 tag / 创建 Release 前的二次确认。

.EXAMPLE
    ./scripts/pack-release.ps1 -Version v1.0.0
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$Version,

    [switch]$Force
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
Set-Location $repoRoot

Write-Host "==> 仓库根目录: $repoRoot" -ForegroundColor Cyan

if (-not $Force) {
    $status = git status --porcelain
    if ($status) {
        Write-Host "工作区存在未提交的更改，请先提交或使用 -Force 跳过此检查：" -ForegroundColor Red
        Write-Host $status
        exit 1
    }
}

$sourceFolder = Join-Path $repoRoot "McpForUnityChinese"
if (-not (Test-Path $sourceFolder)) {
    Write-Host "找不到补丁源码目录: $sourceFolder" -ForegroundColor Red
    exit 1
}

$distDir = Join-Path $repoRoot "dist"
New-Item -ItemType Directory -Force -Path $distDir | Out-Null

$zipName = "McpForUnityChinese-$Version.zip"
$zipPath = Join-Path $distDir $zipName

if (Test-Path $zipPath) {
    Remove-Item $zipPath -Force
}

Write-Host "==> 打包 $sourceFolder -> $zipPath" -ForegroundColor Cyan
Compress-Archive -Path $sourceFolder -DestinationPath $zipPath -CompressionLevel Optimal

Write-Host "==> 打包完成: $zipPath" -ForegroundColor Green

if (-not $Force) {
    $confirm = Read-Host "即将创建并推送 tag '$Version'，确认继续？(y/N)"
    if ($confirm -ne "y" -and $confirm -ne "Y") {
        Write-Host "已取消，未创建 tag。压缩包已保留在 $zipPath" -ForegroundColor Yellow
        exit 0
    }
}

Write-Host "==> 创建 tag $Version" -ForegroundColor Cyan
git tag $Version

Write-Host "==> 推送 tag 到 origin" -ForegroundColor Cyan
git push origin $Version

if (-not $Force) {
    $confirm = Read-Host "即将创建 GitHub Release '$Version' 并上传压缩包，确认继续？(y/N)"
    if ($confirm -ne "y" -and $confirm -ne "Y") {
        Write-Host "已取消，未创建 Release。tag 已推送，压缩包已保留在 $zipPath" -ForegroundColor Yellow
        exit 0
    }
}

Write-Host "==> 创建 GitHub Release" -ForegroundColor Cyan
gh release create $Version $zipPath --title $Version --generate-notes

Write-Host "==> 完成！Release: $Version" -ForegroundColor Green
