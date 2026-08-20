# Security policy

## Reporting a vulnerability

Report privately through GitHub's **Report a vulnerability** button under this
repository's Security tab, which opens a private advisory visible only to the
maintainers. Please do not open a public issue for a suspected vulnerability.

Include the affected package and version, what an attacker can achieve, and a
reproduction if you have one.

You can expect an acknowledgement within 7 days, an assessment within 14, and
credit in the advisory and changelog unless you ask otherwise.

## Supported versions

Only the latest released minor of each package receives security fixes. These are
pre-1.0 libraries and there are no long-term support branches.

## Scope

This library reads no input it did not receive from its caller, opens no file,
network socket or OS store, and stores nothing. It renders a string to the
terminal. That makes the attack surface narrow, and worth stating precisely
rather than waving at.

The one real boundary is **terminal control sequences**. The rendered splash is a
Spectre.Console markup string emitted with a single `AnsiConsole.Markup` call, so
anything the caller supplies that reaches the output has to survive markup
escaping. Two paths carry caller-controlled text:

- **`AppName`**, which is rendered through a Figgle font first. Figgle glyphs
  legitimately contain `[` and `]`, so the renderer doubles both — they become
  markup literals rather than colour tags.
- **`SplashTagline.FromProvider`**, whose return value is escaped the same way
  after word-wrapping and padding.

A caller-supplied string that escaped that handling and reached the terminal as a
live escape sequence would be in scope: on some terminals control sequences can
reposition the cursor, alter the window title, or in the worst historical cases
be replayed as input. If you can make either path emit an unescaped `[` that
Spectre interprets, or get a raw ESC through, that is a report worth filing.

Two things are explicitly **not** claimed:

- **The tagline provider is trusted code, not untrusted input.** `FromProvider`
  takes a `Func<string?>` the consumer wrote. A consumer that pipes unvalidated
  remote data into it owns that decision; the library escapes markup but does not
  sanitise arbitrary control characters.
- **Nothing here is a security control.** There is no secret, no credential, no
  persistence, and no privilege boundary being enforced. A splash screen cannot
  protect anything, so no report should assume it was trying to.

Reports demonstrating a break *within* those stated boundaries are in scope and
welcome. Reports that only restate a documented limitation are not
vulnerabilities.
