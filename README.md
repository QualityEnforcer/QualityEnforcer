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

    QualityEnforcer.exe [options...] path/to/project/

The default option is to detect line endings and indentation style, and use the most common, as well as these
options:

* Trim trailing whitespace
* Trim trailing lines

### Options

**--analysis `file`:** Quality Enforcer will not make any changes, but will instead analyze the project and
produce a summary in markdown format at `file`.

**--rules `file`:** Instead of using CONTRIBUTING.md at the project root, this file will be used instead.

**--summary `file`:** Outputs a summary of the changes in markdown format to `file`.

## Quality Rules

Optionally, Quality Enforcer can pull rules from the CONTRIBUTING.md file in the project. Because this file is
usually intented for human reading, the quality rules format is generally human-readable. Feel free to examine
this repository's CONTRIBUTING.md for an example. Rules take this form:

* Rule name: value

For boolean options, merely including the option name will be sufficient, no need to include 'true'. You may omit
any values you wish, and the default option is to attempt to detect the style your project already uses. All of
these are case-insensitive.

Available options include:

* **Line endings:** CRLF|LF
* **Indentation:** Tabs|\[number] spaces
* **Trim trailing lines**
* **Trim trailing whitespace**

[Click here](https://github.com/blog/1184-contributing-guidelines) to learn why I went with "CONTRIBUTING.md" as
the quality rule file name. You can always override this with --rules.