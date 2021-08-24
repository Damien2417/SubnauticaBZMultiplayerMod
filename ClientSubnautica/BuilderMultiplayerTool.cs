using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using FMODUnity;
using UnityEngine;
using UWE;
namespace ClientSubnautica
{
	class BuilderMultiplayerTool : PlayerTool
	{
		// Token: 0x06003E1E RID: 15902 RVA: 0x00159C16 File Offset: 0x00157E16
		private void Start()
		{
			if (this.barMaterial == null)
			{
				this.barMaterial = this.bar.materials[this.barMaterialID];
			}
			this.SetBeamActive(false);
		}

		// Token: 0x06003E1F RID: 15903 RVA: 0x00159C45 File Offset: 0x00157E45
		private void OnDisable()
		{
			this.buildSound.Stop();
		}

		// Token: 0x06003E20 RID: 15904 RVA: 0x00159C52 File Offset: 0x00157E52
		private void Update()
		{
			this.HandleInput();
		}

		// Token: 0x06003E21 RID: 15905 RVA: 0x00159C5C File Offset: 0x00157E5C
		private void LateUpdate()
		{
			Quaternion b = Quaternion.identity;
			Quaternion b2 = Quaternion.identity;
			bool flag = this.constructable != null;
			if (this.isConstructing != flag)
			{
				this.isConstructing = flag;
				if (this.isConstructing)
				{
					this.leftConstructionInterval = UnityEngine.Random.Range(this.pointSwitchTimeMin, this.pointSwitchTimeMax);
					this.rightConstructionInterval = UnityEngine.Random.Range(this.pointSwitchTimeMin, this.pointSwitchTimeMax);
					this.leftConstructionPoint = this.constructable.GetRandomConstructionPoint();
					this.rightConstructionPoint = this.constructable.GetRandomConstructionPoint();
				}
				else
				{
					this.leftConstructionTime = 0f;
					this.rightConstructionTime = 0f;
				}
			}
			else if (this.isConstructing)
			{
				this.leftConstructionTime += Time.deltaTime;
				this.rightConstructionTime += Time.deltaTime;
				if (this.leftConstructionTime >= this.leftConstructionInterval)
				{
					this.leftConstructionTime %= this.leftConstructionInterval;
					this.leftConstructionInterval = UnityEngine.Random.Range(this.pointSwitchTimeMin, this.pointSwitchTimeMax);
					this.leftConstructionPoint = this.constructable.GetRandomConstructionPoint();
				}
				if (this.rightConstructionTime >= this.rightConstructionInterval)
				{
					this.rightConstructionTime %= this.rightConstructionInterval;
					this.rightConstructionInterval = UnityEngine.Random.Range(this.pointSwitchTimeMin, this.pointSwitchTimeMax);
					this.rightConstructionPoint = this.constructable.GetRandomConstructionPoint();
				}
				this.leftPoint = this.nozzleLeft.parent.InverseTransformPoint(this.leftConstructionPoint);
				this.rightPoint = this.nozzleRight.parent.InverseTransformPoint(this.rightConstructionPoint);
				Debug.DrawLine(this.nozzleLeft.position, this.leftConstructionPoint, Color.white);
				Debug.DrawLine(this.nozzleRight.position, this.rightConstructionPoint, Color.white);
			}
			if (this.isConstructing)
			{
				b = Quaternion.LookRotation(this.leftPoint, Vector3.up);
				b2 = Quaternion.LookRotation(this.rightPoint, Vector3.up);
				Vector3 localScale = this.beamLeft.localScale;
				localScale.z = this.leftPoint.magnitude;
				this.beamLeft.localScale = localScale;
				localScale = this.beamRight.localScale;
				localScale.z = this.rightPoint.magnitude;
				this.beamRight.localScale = localScale;
				Debug.DrawLine(this.nozzleLeft.position, this.leftConstructionPoint, Color.white);
				Debug.DrawLine(this.nozzleRight.position, this.rightConstructionPoint, Color.white);
			}
			float t = this.nozzleRotationSpeed * Time.deltaTime;
			this.nozzleLeft.localRotation = Quaternion.Slerp(this.nozzleLeft.localRotation, b, t);
			this.nozzleRight.localRotation = Quaternion.Slerp(this.nozzleRight.localRotation, b2, t);
			this.SetBeamActive(this.isConstructing);
			this.SetUsingAnimation(this.isConstructing);
			if (this.isConstructing)
			{
				this.buildSound.Play();
			}
			else
			{
				this.buildSound.Stop();
			}
			this.UpdateBar();
			this.constructable = null;
		}

		// Token: 0x06003E22 RID: 15906 RVA: 0x00159F80 File Offset: 0x00158180
		private void HandleInput()
		{
			if (this.handleInputFrame == Time.frameCount)
			{
				return;
			}
			this.handleInputFrame = Time.frameCount;
			if (!base.isDrawn || Builder.isPlacing || !AvatarInputHandler.main.IsEnabled())
			{
				return;
			}
			if (this.TryDisplayNoPowerTooltip())
			{
				return;
			}
			Targeting.AddToIgnoreList(Player.main.gameObject);
			if (gameObject == null)
			{
				return;
			}
			Constructable constructable = gameObject.GetComponentInParent<Constructable>();
			if (constructable != null && num > constructable.placeMaxDistance)
			{
				constructable = null;
			}
			if (constructable != null)
			{
				this.OnHover(constructable);
				if (buttonHeld)
				{
					this.Construct(constructable, true, false);
					return;
				}
				string text;
				if (constructable.DeconstructionAllowed(out text))
				{
					if (buttonHeld2)
					{
						if (constructable.constructed)
						{
							Builder.ResetLast();
							constructable.SetState(false, false);
							return;
						}
						this.Construct(constructable, false, buttonDown);
						return;
					}
				}
				else if (buttonDown && !string.IsNullOrEmpty(text))
				{
					RuntimeManager.PlayOneShot("event:/bz/ui/item_error", default(Vector3));
					ErrorMessage.AddMessage(text);
					return;
				}
			}
			else
			{
				BaseDeconstructable baseDeconstructable = gameObject.GetComponentInParent<BaseDeconstructable>();
				if (baseDeconstructable == null)
				{
					BaseExplicitFace componentInParent = gameObject.GetComponentInParent<BaseExplicitFace>();
					if (componentInParent != null)
					{
						baseDeconstructable = componentInParent.parent;
					}
				}
				if (baseDeconstructable != null && num <= 11f)
				{
					string text;
					if (baseDeconstructable.DeconstructionAllowed(out text))
					{
						this.OnHover(baseDeconstructable);
						if (buttonDown)
						{
							Builder.ResetLast();
							baseDeconstructable.Deconstruct();
							return;
						}
					}
					else if (buttonDown && !string.IsNullOrEmpty(text))
					{
						RuntimeManager.PlayOneShot("event:/bz/ui/item_error", default(Vector3));
						ErrorMessage.AddMessage(text);
					}
				}
			}
		}

		public void Trigger(bool __buttonHeld, bool __buttonDown, bool __buttonHeld2, GameObject __gameObject, float __num)
		{
			buttonHeld = __buttonHeld;
			buttonDown = __buttonDown;
			buttonHeld2 = __buttonHeld2;
			gameObject = __gameObject;
			num = __num;
			HandleInput();
		}
		// Token: 0x06003E23 RID: 15907 RVA: 0x0015A138 File Offset: 0x00158338
		private bool TryDisplayNoPowerTooltip()
		{
			if (!this.HasEnergyOrInBase())
			{
				HandReticle main = HandReticle.main;
				main.SetText(HandReticle.TextType.Hand, Language.main.Get("NoPower"), false, GameInput.Button.None);
				main.SetText(HandReticle.TextType.HandSubscript, string.Empty, false, GameInput.Button.None);
				main.SetIcon(HandReticle.IconType.Default, 1f);
				return true;
			}
			return false;
		}

		// Token: 0x06003E24 RID: 15908 RVA: 0x0015A188 File Offset: 0x00158388
		public override bool OnRightHandDown()
		{
			if (Player.main.IsSpikyTrapAttached())
			{
				return true;
			}
			if (!this.HasEnergyOrInBase())
			{
				return false;
			}
			uGUI_BuilderMenu.Show();
			return true;
		}

		// Token: 0x06003E25 RID: 15909 RVA: 0x0015A1A8 File Offset: 0x001583A8
		private bool HasEnergyOrInBase()
		{
			return Player.main.IsInSub() || this.energyMixin.charge > 0f;
		}

		// Token: 0x06003E26 RID: 15910 RVA: 0x0015A1CA File Offset: 0x001583CA
		public override bool OnLeftHandDown()
		{
			this.HandleInput();
			return this.isConstructing;
		}

		// Token: 0x06003E27 RID: 15911 RVA: 0x0015A1CA File Offset: 0x001583CA
		public override bool OnLeftHandHeld()
		{
			this.HandleInput();
			return this.isConstructing;
		}

		// Token: 0x06003E28 RID: 15912 RVA: 0x0015A1CA File Offset: 0x001583CA
		public override bool OnLeftHandUp()
		{
			this.HandleInput();
			return this.isConstructing;
		}

		// Token: 0x06003E29 RID: 15913 RVA: 0x0015A1D8 File Offset: 0x001583D8
		private void Construct(Constructable c, bool state, bool start = false)
		{
			if (c != null && !c.constructed && this.HasEnergyOrInBase())
			{
				CoroutineHost.StartCoroutine(this.ConstructAsync(c, state, start));
			}
		}

		// Token: 0x06003E2A RID: 15914 RVA: 0x0015A202 File Offset: 0x00158402
		private IEnumerator ConstructAsync(Constructable c, bool state, bool start)
		{
			float amount = (state ? this.powerConsumptionConstruct : this.powerConsumptionDeconstruct) * Time.deltaTime;
			this.energyMixin.ConsumeEnergy(amount);
			bool wasConstructed = c.constructed;
			bool flag;
			if (state)
			{
				flag = c.Construct();
				if (!flag && !wasConstructed)
				{
					global::Utils.PlayFMODAsset(this.completeSound, c.transform, 20f);
				}
			}
			else
			{
				TaskResult<bool> result = new TaskResult<bool>();
				TaskResult<string> resultReason = new TaskResult<string>();
				Vector3 deconstructedPosition = c.transform.position;
				yield return c.DeconstructAsync(result, resultReason);
				flag = result.Get();
				if (c.constructedAmount <= 0f)
				{
					global::Utils.PlayFMODAsset(this.deconstructCompleteSound, deconstructedPosition, 20f);
				}
				if (!flag && (start || this.isConstructing))
				{
					string text = resultReason.Get();
					if (!string.IsNullOrEmpty(text))
					{
						ErrorMessage.AddError(text);
						RuntimeManager.PlayOneShot("event:/bz/ui/item_error", default(Vector3));
					}
				}
				result = null;
				resultReason = null;
				deconstructedPosition = default(Vector3);
			}
			bool flag2 = this.usingPlayer != null;
			if (flag && flag2)
			{
				this.constructable = c;
			}
			if (!wasConstructed && c.constructed)
			{
				TechType techType = c.techType;
				Vector3 deconstructedPosition = c.transform.position;
				if (flag2 && Builder.lastTechType != TechType.None && c.techType == Builder.lastTechType)
				{
					yield return Builder.BeginAsync(Builder.lastTechType);
				}
				CraftingAnalytics.main.OnConstruct(techType, deconstructedPosition);
				deconstructedPosition = default(Vector3);
			}
			yield break;
		}

		// Token: 0x06003E2B RID: 15915 RVA: 0x0015A228 File Offset: 0x00158428
		private void OnHover(Constructable constructable)
		{
			if (constructable.constructed && !constructable.deconstructionAllowed)
			{
				return;
			}
			HandReticle main = HandReticle.main;
			string buttonFormat = LanguageCache.GetButtonFormat("DeconstructFormat", GameInput.Button.Deconstruct);
			if (constructable.constructed)
			{
				HandReticle.main.SetText(HandReticle.TextType.Hand, Language.main.Get(constructable.techType), false, GameInput.Button.None);
				HandReticle.main.SetText(HandReticle.TextType.HandSubscript, buttonFormat, false, GameInput.Button.None);
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(Language.main.GetFormat<string, string>("ConstructDeconstructFormat", LanguageCache.GetButtonFormat("ConstructFormat", GameInput.Button.LeftHand), buttonFormat));
			foreach (KeyValuePair<TechType, int> keyValuePair in constructable.GetRemainingResources())
			{
				TechType key = keyValuePair.Key;
				string text = Language.main.Get(key);
				int value = keyValuePair.Value;
				if (value > 1)
				{
					stringBuilder.AppendLine(Language.main.GetFormat<string, int>("RequireMultipleFormat", text, value));
				}
				else
				{
					stringBuilder.AppendLine(text);
				}
			}
			main.SetText(HandReticle.TextType.Hand, Language.main.Get(constructable.techType), false, GameInput.Button.None);
			main.SetText(HandReticle.TextType.HandSubscript, stringBuilder.ToString(), false, GameInput.Button.None);
			main.SetProgress(constructable.amount);
			main.SetIcon(HandReticle.IconType.Progress, 1.5f);
		}

		// Token: 0x06003E2C RID: 15916 RVA: 0x0015A384 File Offset: 0x00158584
		private void OnHover(BaseDeconstructable deconstructable)
		{
			HandReticle main = HandReticle.main;
			main.SetText(HandReticle.TextType.Hand, deconstructable.Name, true, GameInput.Button.None);
			main.SetText(HandReticle.TextType.HandSubscript, LanguageCache.GetButtonFormat("DeconstructFormat", GameInput.Button.Deconstruct), false, GameInput.Button.None);
		}

		// Token: 0x06003E2D RID: 15917 RVA: 0x0015A3AF File Offset: 0x001585AF
		public override bool GetUsedToolThisFrame()
		{
			return this.isConstructing;
		}

		// Token: 0x06003E2E RID: 15918 RVA: 0x0015A3B7 File Offset: 0x001585B7
		public override void OnDraw(Player p)
		{
			base.OnDraw(p);
			this.wasPlacing = false;
			this.wasPlacingRotatable = false;
			this.UpdateCustomUseText();
			GameInput.OnBindingsChanged += this.UpdateCustomUseText;
		}

		// Token: 0x06003E2F RID: 15919 RVA: 0x0015A3E5 File Offset: 0x001585E5
		public override void OnHolster()
		{
			base.OnHolster();
			if (uGUI_BuilderMenu.IsOpen())
			{
				uGUI_BuilderMenu.Hide();
			}
			Builder.End();
			this.SetBeamActive(false);
			GameInput.OnBindingsChanged -= this.UpdateCustomUseText;
		}

		// Token: 0x06003E30 RID: 15920 RVA: 0x0015A418 File Offset: 0x00158618
		public override string GetCustomUseText()
		{
			bool isPlacing = Builder.isPlacing;
			bool flag = isPlacing && Builder.canRotate;
			if (isPlacing != this.wasPlacing || flag != this.wasPlacingRotatable)
			{
				this.UpdateCustomUseText();
				this.wasPlacing = isPlacing;
				this.wasPlacingRotatable = flag;
			}
			return this.customUseText;
		}

		// Token: 0x06003E31 RID: 15921 RVA: 0x0015A464 File Offset: 0x00158664
		private void UpdateCustomUseText()
		{
			if (Builder.isPlacing)
			{
				this.customUseText = Language.main.GetFormat<string, string>("BuilderWithGhostFormat", uGUI.FormatButton(GameInput.Button.LeftHand, false, " / ", false), uGUI.FormatButton(GameInput.Button.RightHand, false, " / ", false));
				if (Builder.canRotate)
				{
					string format = Language.main.GetFormat<string, string>("GhostRotateInputHint", uGUI.FormatButton(GameInput.Button.CyclePrev, true, ", ", false), uGUI.FormatButton(GameInput.Button.CycleNext, true, ", ", false));
					this.customUseText = string.Format("{0}\n{1}", this.customUseText, format);
					return;
				}
			}
			else
			{
				this.customUseText = LanguageCache.GetButtonFormat("BuilderUseFormat", GameInput.Button.RightHand);
			}
		}

		// Token: 0x06003E32 RID: 15922 RVA: 0x0015A504 File Offset: 0x00158704
		private void UpdateBar()
		{
			if (this.bar == null)
			{
				return;
			}
			float value = (this.energyMixin.capacity > 0f) ? (this.energyMixin.charge / this.energyMixin.capacity) : 0f;
			this.barMaterial.SetFloat(ShaderPropertyID._Amount, value);
		}

		// Token: 0x06003E33 RID: 15923 RVA: 0x0015A562 File Offset: 0x00158762
		private void SetBeamActive(bool state)
		{
			if (this.beamLeft != null)
			{
				this.beamLeft.gameObject.SetActive(state);
			}
			if (this.beamRight != null)
			{
				this.beamRight.gameObject.SetActive(state);
			}
		}

		// Token: 0x06003E34 RID: 15924 RVA: 0x0015A5A2 File Offset: 0x001587A2
		private void SetUsingAnimation(bool state)
		{
			if (this.animator == null || !this.animator.isActiveAndEnabled)
			{
				return;
			}
			SafeAnimator.SetBool(this.animator, "using_tool", state);
		}

		// Token: 0x04003CC9 RID: 15561
		private const float hitRange = 30f;

		// Token: 0x04003CCA RID: 15562
		[AssertLocalization(1)]
		private const string constructFormat = "ConstructFormat";

		// Token: 0x04003CCB RID: 15563
		[AssertLocalization(1)]
		private const string deconstructFormat = "DeconstructFormat";

		// Token: 0x04003CCC RID: 15564
		[AssertLocalization(2)]
		private const string constructDeconstructFormat = "ConstructDeconstructFormat";

		// Token: 0x04003CCD RID: 15565
		[AssertLocalization]
		private const string noPowerKey = "NoPower";

		// Token: 0x04003CCE RID: 15566
		[AssertLocalization(2)]
		private const string requireMultipleFormat = "RequireMultipleFormat";

		// Token: 0x04003CCF RID: 15567
		[AssertLocalization(1)]
		private const string builderUse = "BuilderUseFormat";

		// Token: 0x04003CD0 RID: 15568
		[AssertLocalization(2)]
		private const string builderConstructCancel = "BuilderWithGhostFormat";

		// Token: 0x04003CD1 RID: 15569
		[AssertLocalization(2)]
		private const string builderRotate = "GhostRotateInputHint";

		// Token: 0x04003CD2 RID: 15570
		private const float deconstructRange = 11f;

		// Token: 0x04003CD3 RID: 15571
		public float powerConsumptionConstruct = 0.5f;

		// Token: 0x04003CD4 RID: 15572
		public float powerConsumptionDeconstruct = 0.5f;

		// Token: 0x04003CD5 RID: 15573
		public Renderer bar;

		// Token: 0x04003CD6 RID: 15574
		public int barMaterialID = 1;

		// Token: 0x04003CD7 RID: 15575
		public Transform nozzleLeft;

		// Token: 0x04003CD8 RID: 15576
		public Transform nozzleRight;

		// Token: 0x04003CD9 RID: 15577
		public Transform beamLeft;

		// Token: 0x04003CDA RID: 15578
		public Transform beamRight;

		// Token: 0x04003CDB RID: 15579
		public float nozzleRotationSpeed = 10f;

		// Token: 0x04003CDC RID: 15580
		[Range(0.01f, 5f)]
		public float pointSwitchTimeMin = 0.1f;

		// Token: 0x04003CDD RID: 15581
		[Range(0.01f, 5f)]
		public float pointSwitchTimeMax = 1f;

		// Token: 0x04003CDE RID: 15582
		public Animator animator;

		// Token: 0x04003CDF RID: 15583
		public FMOD_CustomLoopingEmitter buildSound;

		// Token: 0x04003CE0 RID: 15584
		[AssertNotNull]
		public FMODAsset completeSound;

		// Token: 0x04003CE1 RID: 15585
		[AssertNotNull]
		public FMODAsset deconstructCompleteSound;

		// Token: 0x04003CE2 RID: 15586
		private bool isConstructing;

		// Token: 0x04003CE3 RID: 15587
		private Constructable constructable;

		// Token: 0x04003CE4 RID: 15588
		private int handleInputFrame = -1;

		// Token: 0x04003CE5 RID: 15589
		private Vector3 leftPoint = Vector3.zero;

		// Token: 0x04003CE6 RID: 15590
		private Vector3 rightPoint = Vector3.zero;

		// Token: 0x04003CE7 RID: 15591
		private float leftConstructionTime;

		// Token: 0x04003CE8 RID: 15592
		private float rightConstructionTime;

		// Token: 0x04003CE9 RID: 15593
		private float leftConstructionInterval;

		// Token: 0x04003CEA RID: 15594
		private float rightConstructionInterval;

		// Token: 0x04003CEB RID: 15595
		private Vector3 leftConstructionPoint;

		// Token: 0x04003CEC RID: 15596
		private Vector3 rightConstructionPoint;

		// Token: 0x04003CED RID: 15597
		private string customUseText;

		// Token: 0x04003CEE RID: 15598
		private bool wasPlacing;

		// Token: 0x04003CEF RID: 15599
		private bool wasPlacingRotatable;

		// Token: 0x04003CF0 RID: 15600
		private Material barMaterial;

		private bool buttonHeld;
		private bool buttonDown;
		private bool buttonHeld2;
		private GameObject gameObject;
		private float num;
	}
}
