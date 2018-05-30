using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseMenuPopulator : MenuPopulator<ResponseDisplay, string> {
    public List<string> selections;

    public StringUnityEvent onSubmit;
}
