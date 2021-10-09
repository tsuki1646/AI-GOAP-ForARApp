public class Register : GAction {
    public override bool PrePerform() {

        return true;
    }

    public override bool PostPerform() {

        beliefs.ModifyState("atHospital", 0);
        return true;
    }

    public override bool PostPerformCleanUp()
    {
        throw new System.NotImplementedException();
    }
}
