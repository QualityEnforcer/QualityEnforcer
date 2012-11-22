# Quality Enforcer

Quality Enforcer is a tool for making coding styles consistent across projects. It handles things such as:

* Line ending style (LF versus CRLF)
* Indentation style (tabs versus spaces)
* Trailing new lines
* Trailing whitespace

It can also do some basic code analysis, such as how much of a certain style your project uses. Extra fancy
features include:

* Generating language division statistics
* Detecting simple mistakes like adding an extra space of indentation
* Generating indentation maps of files

Quality Enforcer may also be used as a C# library for more customized quality control.

## Usage

**NOTE**: Not implemented

    QualityEnforcer.exe [options...] path/to/project/

The default option is to detect line endings and indentation style, and use the most common, as well as these
options:

* Trim trailing whitespace
* Trim trailing lines

### Options

**--analysis `file`:** Quality Enforcer will not make any changes, but will instead analyze the project and
produce a summary in markdown format at `file`.

**--options `file`:** Instead of using .qualityrules at the project root, this file will be used instead.

**--summary `file`:** Outputs a summary of the changes in markdown format to `file`.

## Quality Rules

Optionally, a `.qualityrules` file may be placed in the root of the project to pull code styles from. This file
is simple, following the `key=value` format. An example:

    # Comments may be made like this. Empty lines are ignored.

    LineEndings=LF
    Indentation=Spaces
    NumberOfSpaces=4

Available options include:

* **LineEndings:** Detect/LF/CRLF. Default: Detect
* **Indentation:** Detect/Tabs/Spaces. Default: Detect
* **NumberOfSpaces:** Integer. For use with space-style indentation. Default: 4
* **TrimTrailingLines:** Boolean. Default: True
* **TrimTrailingWhitespace:** Boolean. Default: True